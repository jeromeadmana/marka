using marka_api.Data;
using marka_api.Interfaces;
using marka_api.Models;
using marka_api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace marka_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid customer ID.");
            }

            var result = await _customerRepository.DeleteCustomer(id);

            if (!result)
            {
                return NotFound("Customer not found.");
            }

            return NoContent();
        }
    }
}
