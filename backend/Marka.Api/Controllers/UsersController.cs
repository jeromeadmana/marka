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
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetUsers()
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userCustomerId = User.FindFirst("CustomerId")?.Value;

        // Regular users cannot access this endpoint
        if (userRole == "User")
        {
            return Forbid();
        }

        IQueryable<User> query = _context.Users.Where(u => !u.IsDeleted);

        // CustomerAdmin can only see users from their own customer
        if (userRole == "CustomerAdmin" && !string.IsNullOrEmpty(userCustomerId))
        {
            var customerId = Guid.Parse(userCustomerId);
            query = query.Where(u => u.CustomerId == customerId);
        }

        // SuperAdmin can see all users (no additional filter needed)

        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.Role,
                u.CustomerId,
                CustomerName = _context.Customers
                    .Where(c => c.Id == u.CustomerId)
                    .Select(c => c.Name)
                    .FirstOrDefault(),
                u.IsActive,
                u.CreatedAt,
                u.UpdatedAt
            })
            .ToListAsync();

        return Ok(users);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetUser(Guid id)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userCustomerId = User.FindFirst("CustomerId")?.Value;
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _context.Users
            .Where(u => u.Id == id && !u.IsDeleted)
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.Role,
                u.CustomerId,
                CustomerName = _context.Customers
                    .Where(c => c.Id == u.CustomerId)
                    .Select(c => c.Name)
                    .FirstOrDefault(),
                u.IsActive,
                u.CreatedAt,
                u.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        // Regular User can only view their own profile
        if (userRole == "User" && currentUserId != id.ToString())
        {
            return Forbid();
        }

        // CustomerAdmin can only view users from their own customer
        if (userRole == "CustomerAdmin" && userCustomerId != user.CustomerId.ToString())
        {
            return Forbid();
        }

        // SuperAdmin can view any user (no restriction)

        return Ok(user);
    }

    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<object>> CreateUser([FromBody] CreateUserRequest request)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userCustomerId = User.FindFirst("CustomerId")?.Value;

        // Regular users cannot create users
        if (userRole == "User")
        {
            return Forbid();
        }

        // Validate request
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new { message = "Email is required" });
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "Password is required" });
        }

        if (string.IsNullOrWhiteSpace(request.FirstName))
        {
            return BadRequest(new { message = "First name is required" });
        }

        if (string.IsNullOrWhiteSpace(request.LastName))
        {
            return BadRequest(new { message = "Last name is required" });
        }

        if (request.CustomerId == Guid.Empty)
        {
            return BadRequest(new { message = "Customer ID is required" });
        }

        // Check if customer exists
        var customerExists = await _context.Customers
            .AnyAsync(c => c.Id == request.CustomerId && !c.IsDeleted);

        if (!customerExists)
        {
            return BadRequest(new { message = "Customer not found" });
        }

        // CustomerAdmin can only create users for their own customer
        if (userRole == "CustomerAdmin")
        {
            if (userCustomerId != request.CustomerId.ToString())
            {
                return Forbid();
            }

            // CustomerAdmin cannot create SuperAdmin or CustomerAdmin users
            if (request.Role == UserRole.SuperAdmin || request.Role == UserRole.CustomerAdmin)
            {
                return Forbid();
            }
        }

        // SuperAdmin can create users with any role, but validate requested role
        if (userRole == "SuperAdmin")
        {
            // SuperAdmin creating another SuperAdmin
            if (request.Role == UserRole.SuperAdmin)
            {
                // Allow - SuperAdmin can create other SuperAdmins
            }
            // SuperAdmin creating CustomerAdmin
            else if (request.Role == UserRole.CustomerAdmin)
            {
                // Allow - SuperAdmin can create CustomerAdmins
            }
            // SuperAdmin creating regular User
            else if (request.Role == UserRole.User)
            {
                // Allow - SuperAdmin can create regular Users
            }
        }

        // Check if user with same email already exists
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower() && !u.IsDeleted);

        if (existingUser != null)
        {
            return BadRequest(new { message = "User with this email already exists" });
        }

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email.ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = request.Role,
            CustomerId = request.CustomerId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetUser),
            new { id = newUser.Id },
            new
            {
                newUser.Id,
                newUser.FirstName,
                newUser.LastName,
                newUser.Email,
                newUser.Role,
                newUser.CustomerId,
                newUser.IsActive,
                newUser.CreatedAt,
                newUser.UpdatedAt
            });
    }

    // PUT: api/users/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<object>> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userCustomerId = User.FindFirst("CustomerId")?.Value;
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        // Regular User can only update their own profile (limited fields)
        if (userRole == "User")
        {
            if (currentUserId != id.ToString())
            {
                return Forbid();
            }

            // Regular users can only update their own name and password
            if (!string.IsNullOrWhiteSpace(request.FirstName))
            {
                user.FirstName = request.FirstName;
            }

            if (!string.IsNullOrWhiteSpace(request.LastName))
            {
                user.LastName = request.LastName;
            }

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            }

            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Role,
                user.CustomerId,
                user.IsActive,
                user.CreatedAt,
                user.UpdatedAt
            });
        }

        // CustomerAdmin can only update users from their own customer
        if (userRole == "CustomerAdmin" && userCustomerId != user.CustomerId.ToString())
        {
            return Forbid();
        }

        // CustomerAdmin cannot change user roles to SuperAdmin or CustomerAdmin
        if (userRole == "CustomerAdmin" && request.Role.HasValue)
        {
            if (request.Role.Value == UserRole.SuperAdmin || request.Role.Value == UserRole.CustomerAdmin)
            {
                return Forbid();
            }
        }

        // Update fields if provided
        if (!string.IsNullOrWhiteSpace(request.FirstName))
        {
            user.FirstName = request.FirstName;
        }

        if (!string.IsNullOrWhiteSpace(request.LastName))
        {
            user.LastName = request.LastName;
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            // Check if email is being changed to an existing email
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower() && u.Id != id && !u.IsDeleted);

            if (existingUser != null)
            {
                return BadRequest(new { message = "User with this email already exists" });
            }

            user.Email = request.Email.ToLower();
        }

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        }

        if (request.Role.HasValue)
        {
            user.Role = request.Role.Value;
        }

        if (request.IsActive.HasValue)
        {
            user.IsActive = request.IsActive.Value;
        }

        // SuperAdmin can change customer assignment, but CustomerAdmin cannot
        if (request.CustomerId.HasValue && userRole == "SuperAdmin")
        {
            var customerExists = await _context.Customers
                .AnyAsync(c => c.Id == request.CustomerId.Value && !c.IsDeleted);

            if (!customerExists)
            {
                return BadRequest(new { message = "Customer not found" });
            }

            user.CustomerId = request.CustomerId.Value;
        }

        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Role,
            user.CustomerId,
            user.IsActive,
            user.CreatedAt,
            user.UpdatedAt
        });
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userCustomerId = User.FindFirst("CustomerId")?.Value;
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Regular users cannot delete users
        if (userRole == "User")
        {
            return Forbid();
        }

        // Users cannot delete themselves
        if (currentUserId == id.ToString())
        {
            return BadRequest(new { message = "Cannot delete your own account" });
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        // CustomerAdmin can only delete users from their own customer
        if (userRole == "CustomerAdmin")
        {
            if (userCustomerId != user.CustomerId.ToString())
            {
                return Forbid();
            }

            // CustomerAdmin cannot delete SuperAdmin or CustomerAdmin users
            if (user.Role == UserRole.SuperAdmin || user.Role == UserRole.CustomerAdmin)
            {
                return Forbid();
            }
        }

        // Soft delete the user
        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "User deleted successfully" });
    }
}

// Request models
public class CreateUserRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
    public Guid CustomerId { get; set; }
}

public class UpdateUserRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public UserRole? Role { get; set; }
    public Guid? CustomerId { get; set; }
    public bool? IsActive { get; set; }
}
