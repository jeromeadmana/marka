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
public class RolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public RolesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/roles
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetRoles()
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userCustomerId = User.FindFirst("CustomerId")?.Value;

        // Only CustomerAdmin and SuperAdmin can view roles
        if (userRole == "User")
        {
            return Forbid();
        }

        IQueryable<CustomRole> query = _context.CustomRoles.Where(r => !r.IsDeleted);

        // CustomerAdmin can only see roles from their own customer
        if (userRole == "CustomerAdmin" && !string.IsNullOrEmpty(userCustomerId))
        {
            var customerId = Guid.Parse(userCustomerId);
            query = query.Where(r => r.CustomerId == customerId);
        }

        // SuperAdmin can see all roles (no additional filter)

        var roles = await query
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new
            {
                r.Id,
                r.Name,
                r.Description,
                r.CustomerId,
                CustomerName = _context.Customers
                    .Where(c => c.Id == r.CustomerId)
                    .Select(c => c.Name)
                    .FirstOrDefault(),
                r.IsActive,
                r.CreatedAt,
                r.UpdatedAt,
                PermissionCount = _context.RolePermissions
                    .Count(rp => rp.CustomRoleId == r.Id),
                UserCount = _context.Users
                    .Count(u => u.CustomRoleId == r.Id && !u.IsDeleted)
            })
            .ToListAsync();

        return Ok(roles);
    }

    // GET: api/roles/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetRole(Guid id)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userCustomerId = User.FindFirst("CustomerId")?.Value;

        var role = await _context.CustomRoles
            .Where(r => r.Id == id && !r.IsDeleted)
            .Select(r => new
            {
                r.Id,
                r.Name,
                r.Description,
                r.CustomerId,
                CustomerName = _context.Customers
                    .Where(c => c.Id == r.CustomerId)
                    .Select(c => c.Name)
                    .FirstOrDefault(),
                r.IsActive,
                r.CreatedAt,
                r.UpdatedAt,
                Permissions = _context.RolePermissions
                    .Where(rp => rp.CustomRoleId == r.Id)
                    .Select(rp => new
                    {
                        rp.Permission.Id,
                        rp.Permission.Code,
                        rp.Permission.Name,
                        rp.Permission.Description,
                        rp.Permission.Category
                    })
                    .ToList(),
                Users = _context.Users
                    .Where(u => u.CustomRoleId == r.Id && !u.IsDeleted)
                    .Select(u => new
                    {
                        u.Id,
                        u.FirstName,
                        u.LastName,
                        u.Email
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (role == null)
        {
            return NotFound(new { message = "Role not found" });
        }

        // CustomerAdmin can only view roles from their own customer
        if (userRole == "CustomerAdmin" && userCustomerId != role.CustomerId.ToString())
        {
            return Forbid();
        }

        // Regular users cannot view roles
        if (userRole == "User")
        {
            return Forbid();
        }

        return Ok(role);
    }

    // POST: api/roles
    [HttpPost]
    public async Task<ActionResult<object>> CreateRole([FromBody] CreateRoleRequest request)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userCustomerId = User.FindFirst("CustomerId")?.Value;

        // Only CustomerAdmin and SuperAdmin can create roles
        if (userRole == "User")
        {
            return Forbid();
        }

        // Validate request
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "Role name is required" });
        }

        if (request.CustomerId == Guid.Empty)
        {
            return BadRequest(new { message = "Customer ID is required" });
        }

        // CustomerAdmin can only create roles for their own customer
        if (userRole == "CustomerAdmin")
        {
            if (userCustomerId != request.CustomerId.ToString())
            {
                return Forbid();
            }
        }

        // Check if customer exists
        var customerExists = await _context.Customers
            .AnyAsync(c => c.Id == request.CustomerId && !c.IsDeleted);

        if (!customerExists)
        {
            return BadRequest(new { message = "Customer not found" });
        }

        // Check if role with same name already exists for this customer
        var existingRole = await _context.CustomRoles
            .FirstOrDefaultAsync(r =>
                r.CustomerId == request.CustomerId &&
                r.Name.ToLower() == request.Name.ToLower() &&
                !r.IsDeleted);

        if (existingRole != null)
        {
            return BadRequest(new { message = "Role with this name already exists for this customer" });
        }

        var role = new CustomRole
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description ?? string.Empty,
            CustomerId = request.CustomerId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.CustomRoles.Add(role);
        await _context.SaveChangesAsync();

        // Assign permissions if provided
        if (request.PermissionIds != null && request.PermissionIds.Any())
        {
            var validPermissions = await _context.Permissions
                .Where(p => request.PermissionIds.Contains(p.Id) && p.IsActive)
                .Select(p => p.Id)
                .ToListAsync();

            var rolePermissions = validPermissions.Select(permId => new RolePermission
            {
                Id = Guid.NewGuid(),
                CustomRoleId = role.Id,
                PermissionId = permId,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            _context.RolePermissions.AddRange(rolePermissions);
            await _context.SaveChangesAsync();
        }

        return CreatedAtAction(
            nameof(GetRole),
            new { id = role.Id },
            new
            {
                role.Id,
                role.Name,
                role.Description,
                role.CustomerId,
                role.IsActive,
                role.CreatedAt,
                role.UpdatedAt
            });
    }

    // PUT: api/roles/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<object>> UpdateRole(Guid id, [FromBody] UpdateRoleRequest request)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userCustomerId = User.FindFirst("CustomerId")?.Value;

        var role = await _context.CustomRoles
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

        if (role == null)
        {
            return NotFound(new { message = "Role not found" });
        }

        // CustomerAdmin can only update roles from their own customer
        if (userRole == "CustomerAdmin" && userCustomerId != role.CustomerId.ToString())
        {
            return Forbid();
        }

        // Regular users cannot update roles
        if (userRole == "User")
        {
            return Forbid();
        }

        // Update fields if provided
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            // Check if name is being changed to an existing role name
            var existingRole = await _context.CustomRoles
                .FirstOrDefaultAsync(r =>
                    r.CustomerId == role.CustomerId &&
                    r.Name.ToLower() == request.Name.ToLower() &&
                    r.Id != id &&
                    !r.IsDeleted);

            if (existingRole != null)
            {
                return BadRequest(new { message = "Role with this name already exists for this customer" });
            }

            role.Name = request.Name;
        }

        if (request.Description != null)
        {
            role.Description = request.Description;
        }

        if (request.IsActive.HasValue)
        {
            role.IsActive = request.IsActive.Value;
        }

        role.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            role.Id,
            role.Name,
            role.Description,
            role.CustomerId,
            role.IsActive,
            role.CreatedAt,
            role.UpdatedAt
        });
    }

    // POST: api/roles/{id}/permissions
    [HttpPost("{id}/permissions")]
    public async Task<ActionResult> AssignPermissions(Guid id, [FromBody] AssignPermissionsRequest request)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userCustomerId = User.FindFirst("CustomerId")?.Value;

        var role = await _context.CustomRoles
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

        if (role == null)
        {
            return NotFound(new { message = "Role not found" });
        }

        // CustomerAdmin can only manage permissions for roles in their customer
        if (userRole == "CustomerAdmin" && userCustomerId != role.CustomerId.ToString())
        {
            return Forbid();
        }

        // Regular users cannot manage permissions
        if (userRole == "User")
        {
            return Forbid();
        }

        if (request.PermissionIds == null || !request.PermissionIds.Any())
        {
            return BadRequest(new { message = "At least one permission ID is required" });
        }

        // Validate that all permission IDs exist and are active
        var validPermissions = await _context.Permissions
            .Where(p => request.PermissionIds.Contains(p.Id) && p.IsActive)
            .Select(p => p.Id)
            .ToListAsync();

        if (validPermissions.Count != request.PermissionIds.Count)
        {
            return BadRequest(new { message = "One or more permission IDs are invalid or inactive" });
        }

        // Get existing permissions for this role
        var existingPermissions = await _context.RolePermissions
            .Where(rp => rp.CustomRoleId == id)
            .Select(rp => rp.PermissionId)
            .ToListAsync();

        // Find new permissions to add
        var permissionsToAdd = validPermissions.Except(existingPermissions).ToList();

        if (permissionsToAdd.Any())
        {
            var newRolePermissions = permissionsToAdd.Select(permId => new RolePermission
            {
                Id = Guid.NewGuid(),
                CustomRoleId = id,
                PermissionId = permId,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            _context.RolePermissions.AddRange(newRolePermissions);
            await _context.SaveChangesAsync();
        }

        return Ok(new { message = $"Successfully assigned {permissionsToAdd.Count} new permissions to role" });
    }

    // DELETE: api/roles/{id}/permissions/{permissionId}
    [HttpDelete("{id}/permissions/{permissionId}")]
    public async Task<ActionResult> RemovePermission(Guid id, Guid permissionId)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userCustomerId = User.FindFirst("CustomerId")?.Value;

        var role = await _context.CustomRoles
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

        if (role == null)
        {
            return NotFound(new { message = "Role not found" });
        }

        // CustomerAdmin can only manage permissions for roles in their customer
        if (userRole == "CustomerAdmin" && userCustomerId != role.CustomerId.ToString())
        {
            return Forbid();
        }

        // Regular users cannot manage permissions
        if (userRole == "User")
        {
            return Forbid();
        }

        var rolePermission = await _context.RolePermissions
            .FirstOrDefaultAsync(rp => rp.CustomRoleId == id && rp.PermissionId == permissionId);

        if (rolePermission == null)
        {
            return NotFound(new { message = "Permission not assigned to this role" });
        }

        _context.RolePermissions.Remove(rolePermission);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Permission removed from role successfully" });
    }

    // DELETE: api/roles/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRole(Guid id)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userCustomerId = User.FindFirst("CustomerId")?.Value;

        var role = await _context.CustomRoles
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

        if (role == null)
        {
            return NotFound(new { message = "Role not found" });
        }

        // CustomerAdmin can only delete roles from their own customer
        if (userRole == "CustomerAdmin" && userCustomerId != role.CustomerId.ToString())
        {
            return Forbid();
        }

        // Regular users cannot delete roles
        if (userRole == "User")
        {
            return Forbid();
        }

        // Check if any users are assigned to this role
        var usersWithRole = await _context.Users
            .CountAsync(u => u.CustomRoleId == id && !u.IsDeleted);

        if (usersWithRole > 0)
        {
            return BadRequest(new { message = $"Cannot delete role. {usersWithRole} user(s) are currently assigned to this role" });
        }

        // Soft delete the role
        role.IsDeleted = true;
        role.DeletedAt = DateTime.UtcNow;
        role.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Role deleted successfully" });
    }
}

// Request models
public class CreateRoleRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CustomerId { get; set; }
    public List<Guid>? PermissionIds { get; set; }
}

public class UpdateRoleRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}

public class AssignPermissionsRequest
{
    public List<Guid> PermissionIds { get; set; } = new();
}
