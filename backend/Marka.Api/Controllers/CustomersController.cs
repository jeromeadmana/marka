using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Marka.Api.Data;
using Marka.Api.Models;
using System.Security.Claims;

namespace Marka.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/customers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetCustomers()
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        // Only SuperAdmin can view all customers
        if (userRole != "SuperAdmin")
        {
            return Forbid();
        }

        var customers = await _context.Customers
            .Where(c => !c.IsDeleted)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.ContactName,
                c.ContactEmail,
                c.ContactPhone,
                c.IsActive,
                c.CreatedAt,
                c.UpdatedAt,
                UserCount = _context.Users.Count(u => u.CustomerId == c.Id && !u.IsDeleted),
                MarkaCount = _context.Markas.Count(m => m.CustomerId == c.Id)
            })
            .ToListAsync();

        return Ok(customers);
    }

    // GET: api/customers/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetCustomer(Guid id)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userCustomerId = User.FindFirst("CustomerId")?.Value;

        // SuperAdmin can view any customer
        // CustomerAdmin can only view their own customer
        if (userRole == "CustomerAdmin" && userCustomerId != id.ToString())
        {
            return Forbid();
        }

        if (userRole == "User")
        {
            return Forbid();
        }

        var customer = await _context.Customers
            .Where(c => c.Id == id && !c.IsDeleted)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.ContactName,
                c.ContactEmail,
                c.ContactPhone,
                c.IsActive,
                c.CreatedAt,
                c.UpdatedAt,
                Users = _context.Users
                    .Where(u => u.CustomerId == c.Id && !u.IsDeleted)
                    .Select(u => new
                    {
                        u.Id,
                        u.FirstName,
                        u.LastName,
                        u.Email,
                        u.Role,
                        u.IsActive,
                        u.CreatedAt
                    })
                    .ToList(),
                MarkaCount = _context.Markas.Count(m => m.CustomerId == c.Id)
            })
            .FirstOrDefaultAsync();

        if (customer == null)
        {
            return NotFound(new { message = "Customer not found" });
        }

        return Ok(customer);
    }

    // POST: api/customers
    [HttpPost]
    public async Task<ActionResult<object>> CreateCustomer([FromBody] CreateCustomerRequest request)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        // Only SuperAdmin can create customers
        if (userRole != "SuperAdmin")
        {
            return Forbid();
        }

        // Validate request
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "Customer name is required" });
        }

        if (string.IsNullOrWhiteSpace(request.ContactEmail))
        {
            return BadRequest(new { message = "Contact email is required" });
        }

        // Check if customer with same name already exists
        var existingCustomer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Name.ToLower() == request.Name.ToLower() && !c.IsDeleted);

        if (existingCustomer != null)
        {
            return BadRequest(new { message = "Customer with this name already exists" });
        }

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            ContactName = request.ContactName ?? string.Empty,
            ContactEmail = request.ContactEmail,
            ContactPhone = request.ContactPhone ?? string.Empty,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCustomer),
            new { id = customer.Id },
            new
            {
                customer.Id,
                customer.Name,
                customer.ContactName,
                customer.ContactEmail,
                customer.ContactPhone,
                customer.IsActive,
                customer.CreatedAt,
                customer.UpdatedAt
            });
    }

    // PUT: api/customers/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<object>> UpdateCustomer(Guid id, [FromBody] UpdateCustomerRequest request)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userCustomerId = User.FindFirst("CustomerId")?.Value;

        // SuperAdmin can update any customer
        // CustomerAdmin can only update their own customer
        if (userRole == "CustomerAdmin" && userCustomerId != id.ToString())
        {
            return Forbid();
        }

        if (userRole == "User")
        {
            return Forbid();
        }

        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (customer == null)
        {
            return NotFound(new { message = "Customer not found" });
        }

        // Update fields if provided
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            // Check if name is being changed to an existing customer name
            var existingCustomer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Name.ToLower() == request.Name.ToLower() && c.Id != id && !c.IsDeleted);

            if (existingCustomer != null)
            {
                return BadRequest(new { message = "Customer with this name already exists" });
            }

            customer.Name = request.Name;
        }

        if (request.ContactName != null)
        {
            customer.ContactName = request.ContactName;
        }

        if (request.ContactEmail != null)
        {
            customer.ContactEmail = request.ContactEmail;
        }

        if (request.ContactPhone != null)
        {
            customer.ContactPhone = request.ContactPhone;
        }

        if (request.IsActive.HasValue)
        {
            customer.IsActive = request.IsActive.Value;
        }

        customer.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            customer.Id,
            customer.Name,
            customer.ContactName,
            customer.ContactEmail,
            customer.ContactPhone,
            customer.IsActive,
            customer.CreatedAt,
            customer.UpdatedAt
        });
    }

    // DELETE: api/customers/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCustomer(Guid id)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        // Only SuperAdmin can delete customers
        if (userRole != "SuperAdmin")
        {
            return Forbid();
        }

        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (customer == null)
        {
            return NotFound(new { message = "Customer not found" });
        }

        // Soft delete the customer
        customer.IsDeleted = true;
        customer.DeletedAt = DateTime.UtcNow;
        customer.UpdatedAt = DateTime.UtcNow;

        // Also soft delete all users belonging to this customer
        var users = await _context.Users
            .Where(u => u.CustomerId == id && !u.IsDeleted)
            .ToListAsync();

        foreach (var user in users)
        {
            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return Ok(new { message = "Customer deleted successfully" });
    }
}

// Request models
public class CreateCustomerRequest
{
    public string Name { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string ContactEmail { get; set; } = string.Empty;
    public string? ContactPhone { get; set; }
}

public class UpdateCustomerRequest
{
    public string? Name { get; set; }
    public string? ContactName { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public bool? IsActive { get; set; }
}
