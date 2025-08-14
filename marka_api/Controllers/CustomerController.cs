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

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        {
            var customer = await _customerRepository.GetAllCustomers();

            return customer.ToList();
        }
    }
}
