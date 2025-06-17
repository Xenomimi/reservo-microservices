using CustomerServiceApi.Dtos;
using CustomerServiceApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerServiceApi.Services
{
    public class CustomerService
    {
        private CustomerContextDb _context;

        public CustomerService(CustomerContextDb context)
        {
            _context = context;
        }

        public async Task<Customer?> GetById(int id)
        {
            return await _context.Customers
                .Include(c => c.Info)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Customer>> Get()
        {
            return await _context.Customers
                .Include(c => c.Info)
                .ToListAsync();
        }

        public async Task Add(CustomerDto dto)
        {
            var customer = new Customer
            {
                FullName = dto.FullName,
                Info = new CustomerInfo
                {
                    Email = dto.Info.Email,
                    PhoneNumber = dto.Info.PhoneNumber,
                    IsVIP = dto.Info.IsVIP
                }
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
        public async Task Update(int customerId, CustomerDto dto)
        {
            var existing = await _context.Customers
                .Include(c => c.Info)
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (existing != null)
            {
                existing.FullName = dto.FullName;

                if (existing.Info != null && dto.Info != null)
                {
                    existing.Info.Email = dto.Info.Email;
                    existing.Info.PhoneNumber = dto.Info.PhoneNumber;
                    existing.Info.IsVIP = dto.Info.IsVIP;
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
