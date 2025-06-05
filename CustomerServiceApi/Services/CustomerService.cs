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

        public async Task<Customer> GetById(int id)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Customer>> Get()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task Add(Customer entity)
        {
            _context.Set<Customer>()
                    .Add(entity);

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
        public async Task Update(Customer updatedCustomer)
        {
            var existing = await _context.Customers.FindAsync(updatedCustomer.Id);
            if (existing != null)
            {
                existing.FullName = updatedCustomer.FullName;
                existing.Email = updatedCustomer.Email;
                existing.PhoneNumber = updatedCustomer.PhoneNumber;

                await _context.SaveChangesAsync();
            }
        }
    }
}
