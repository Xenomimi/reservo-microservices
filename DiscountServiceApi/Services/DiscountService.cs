using DiscountServiceApi;
using DiscountServiceApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerServiceApi.Services
{
    public class DiscountService
    {
        private DiscountDbContext _context;

        public DiscountService(DiscountDbContext context)
        {
            _context = context;
        }

        public async Task<Discount> GetById(int id)
        {
            return await _context.Discounts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Discount>> Get()
        {
            return await _context.Discounts.ToListAsync();
        }

        public async Task Add(Discount entity)
        {
            _context.Set<Discount>()
                    .Add(entity);

            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var customer = await _context.Discounts.FindAsync(id);
            if (customer != null)
            {
                _context.Discounts.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
        public async Task Update(Discount updatedCustomer)
        {
            var existing = await _context.Discounts.FindAsync(updatedCustomer.Id);
            if (existing != null)
            {
                existing.DiscountPercent = updatedCustomer.DiscountPercent;
                existing.ValidFrom = updatedCustomer.ValidFrom;
                existing.ValidTo = updatedCustomer.ValidTo;

                await _context.SaveChangesAsync();
            }
        }
    }
}
