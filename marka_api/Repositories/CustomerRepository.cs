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
    }
}
