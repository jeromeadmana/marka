using marka_api.Data;
using marka_api.Interfaces;
using marka_api.Models;
using Microsoft.EntityFrameworkCore;

namespace marka_api.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerById(Guid id)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.id == id);
        }

        public async Task<Customer> CreateCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<bool> DeleteCustomer(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return false;
            }

            // Soft delete instead of physical delete
            customer.is_deleted = true;
            customer.deleted_at = DateTime.UtcNow;
            customer.deleted_by = /* current user's ID */ Guid.NewGuid(); // Replace with actual user context

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
