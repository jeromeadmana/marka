using marka_api.Data;
using marka_api.Interfaces;
using marka_api.Models;
using marka_api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace marka_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(AppDbContext context, ICustomerRepository customerRepository)
        {
            _context = context;
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        {
            var customer = await _customerRepository.GetAllCustomers();

            return customer.ToList();
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
        {
            if (customer == null)
            {
                return BadRequest("Customer cannot be null");
            }
            var createdCustomer = await _customerRepository.CreateCustomer(customer);
            return CreatedAtAction(nameof(GetAllCustomers), new { id = createdCustomer.id }, createdCustomer);
        }

        [HttpPatch("{id}")]
        public async Task<bool> DeleteCustomer(Guid id)
        {
            if (id == Guid.Empty)
            {
                return false;
            }
            return await _customerRepository.DeleteCustomer(id);
        }
    }
}
