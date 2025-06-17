using DiscountServiceApi;
using DiscountServiceApi.Dtos;
using DiscountServiceApi.Entities;
using Microsoft.EntityFrameworkCore;
using DiscountStatus = DiscountServiceApi.Entities.DiscountStatus;

namespace DiscountServiceApi.Services
{
    public class DiscountService
    {
        private readonly DiscountDbContext _context;

        public DiscountService(
            DiscountDbContext context)
        {
            _context = context;
        }

        public async Task Add(DiscountDto dto)
        {
            var discount = new Discount
            {
                Code = dto.Code,
                Description = dto.Description,
                Percentage = dto.Percentage,
                RequiresVipCustomer = dto.RequiresVipCustomer
            };

            _context.Discounts.Add(discount);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var discount = await _context.Set<Discount>()
                                         .FirstOrDefaultAsync(d => d.Id == id);
            if (discount == null)
                throw new KeyNotFoundException("Discount not found.");
            _context.Set<Discount>().Remove(discount);
            await _context.SaveChangesAsync();
        }

        public async Task MarkAsUsed(int discountId)
        {
            var discount = await _context.Set<Discount>()
                                         .FirstOrDefaultAsync(d => d.Id == discountId);
            if (discount == null)
                throw new KeyNotFoundException("Discount not found.");
            discount.DiscountStatus = DiscountStatus.Used;
            await _context.SaveChangesAsync();
        }
    }
}
