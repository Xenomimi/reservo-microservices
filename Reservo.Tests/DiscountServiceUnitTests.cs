using DiscountServiceApi.Dtos;
using DiscountServiceApi.Entities;
using DiscountServiceApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DiscountServiceApi.UnitTests
{
    public class DiscountServiceUnitTests
    {

        // Test Delete usuwa istniejący discount
        [Fact]
        public async Task Delete_RemovesDiscount_WhenFound()
        {
            var discountToDelete = new Discount { Id = 1, Code = "PROMO1" };
            var data = new List<Discount> { discountToDelete }.AsQueryable();

            var mockSet = CreateMockDbSet(data);

            mockSet.Setup(m => m.Remove(It.IsAny<Discount>())).Verifiable();

            var options = new DbContextOptionsBuilder<DiscountDbContext>().Options;
            var mockContext = new Mock<DiscountDbContext>(options);
            mockContext.Setup(c => c.Set<Discount>()).Returns(mockSet.Object);
            mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1).Verifiable();

            var service = new DiscountService(mockContext.Object);

            await service.Delete(1);

            mockSet.Verify(m => m.Remove(It.Is<Discount>(d => d.Id == 1)), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // Test Delete rzuca wyjątek gdy nie znaleziono
        [Fact]
        public async Task Delete_Throws_WhenNotFound()
        {
            var data = new List<Discount>().AsQueryable();

            var mockSet = CreateMockDbSet(data);

            var options = new DbContextOptionsBuilder<DiscountDbContext>().Options;
            var mockContext = new Mock<DiscountDbContext>(options);
            mockContext.Setup(c => c.Set<Discount>()).Returns(mockSet.Object);

            var service = new DiscountService(mockContext.Object);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Delete(1));
        }

        // Test MarkAsUsed ustawia status na Used i zapisuje zmiany
        [Fact]
        public async Task MarkAsUsed_UpdatesStatus_WhenFound()
        {
            var discount = new Discount { Id = 1, DiscountStatus = Entities.DiscountStatus.NotUsed };
            var data = new List<Discount> { discount }.AsQueryable();

            var mockSet = CreateMockDbSet(data);

            var options = new DbContextOptionsBuilder<DiscountDbContext>().Options;
            var mockContext = new Mock<DiscountDbContext>(options);
            mockContext.Setup(c => c.Set<Discount>()).Returns(mockSet.Object);
            mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1).Verifiable();

            var service = new DiscountService(mockContext.Object);

            await service.MarkAsUsed(1);

            Assert.Equal(Entities.DiscountStatus.Used, discount.DiscountStatus);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // Test MarkAsUsed rzuca wyjątek gdy discount nie istnieje
        [Fact]
        public async Task MarkAsUsed_Throws_WhenNotFound()
        {
            var data = new List<Discount>().AsQueryable();

            var mockSet = CreateMockDbSet(data);

            var options = new DbContextOptionsBuilder<DiscountDbContext>().Options;
            var mockContext = new Mock<DiscountDbContext>(options);
            mockContext.Setup(c => c.Set<Discount>()).Returns(mockSet.Object);

            var service = new DiscountService(mockContext.Object);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.MarkAsUsed(1));
        }

        // Pomocnik do mockowania DbSet z async (potrzebny do EF Core async LINQ)
        private static Mock<DbSet<Discount>> CreateMockDbSet(IQueryable<Discount> data)
        {
            var mockSet = new Mock<DbSet<Discount>>();

            mockSet.As<IAsyncEnumerable<Discount>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Discount>(data.GetEnumerator()));

            mockSet.As<IQueryable<Discount>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Discount>(data.Provider));
            mockSet.As<IQueryable<Discount>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Discount>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Discount>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            return mockSet;
        }

        // Implementacja IAsyncEnumerator<T> do testów
        internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner;
            public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;
            public T Current => _inner.Current;
            public ValueTask DisposeAsync() => new ValueTask();
            public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());
        }

        // Implementacja IAsyncQueryProvider do testów
        internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
        {
            private readonly IQueryProvider _inner;
            internal TestAsyncQueryProvider(IQueryProvider inner) => _inner = inner;

            public IQueryable CreateQuery(Expression expression) => new TestAsyncEnumerable<TEntity>(expression);
            public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new TestAsyncEnumerable<TElement>(expression);
            public object Execute(Expression expression) => _inner.Execute(expression);
            public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);

            public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
            {
                var expectedResultType = typeof(TResult).GetGenericArguments()[0];
                var result = _inner.Execute(expression);
                return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))!
                    .MakeGenericMethod(expectedResultType)
                    .Invoke(null, new[] { result })!;
            }
        }

        // Implementacja IQueryable<T> i IAsyncEnumerable<T> do testów
        internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
        {
            public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }
            public TestAsyncEnumerable(Expression expression) : base(expression) { }

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

            IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
        }
    }
}
