using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Marka.Api.Data;
using System.Security.Claims;

namespace Marka.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PermissionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PermissionsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/permissions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetPermissions()
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        // Only CustomerAdmin and SuperAdmin can view permissions
        if (userRole == "User")
        {
            return Forbid();
        }

        var permissions = await _context.Permissions
            .Where(p => p.IsActive)
            .OrderBy(p => p.Category)
            .ThenBy(p => p.Name)
            .Select(p => new
            {
                p.Id,
                p.Code,
                p.Name,
                p.Description,
                p.Category,
                p.IsActive
            })
            .ToListAsync();

        return Ok(permissions);
    }

    // GET: api/permissions/categories
    [HttpGet("categories")]
    public async Task<ActionResult<object>> GetPermissionsByCategory()
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        // Only CustomerAdmin and SuperAdmin can view permissions
        if (userRole == "User")
        {
            return Forbid();
        }

        var permissions = await _context.Permissions
            .Where(p => p.IsActive)
            .OrderBy(p => p.Category)
            .ThenBy(p => p.Name)
            .Select(p => new
            {
                p.Id,
                p.Code,
                p.Name,
                p.Description,
                p.Category
            })
            .ToListAsync();

        var groupedPermissions = permissions
            .GroupBy(p => p.Category)
            .Select(g => new
            {
                Category = g.Key,
                Permissions = g.ToList()
            })
            .ToList();

        return Ok(groupedPermissions);
    }

    // GET: api/permissions/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetPermission(Guid id)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        // Only CustomerAdmin and SuperAdmin can view permissions
        if (userRole == "User")
        {
            return Forbid();
        }

        var permission = await _context.Permissions
            .Where(p => p.Id == id && p.IsActive)
            .Select(p => new
            {
                p.Id,
                p.Code,
                p.Name,
                p.Description,
                p.Category,
                p.IsActive,
                RoleCount = _context.RolePermissions
                    .Count(rp => rp.PermissionId == p.Id)
            })
            .FirstOrDefaultAsync();

        if (permission == null)
        {
            return NotFound(new { message = "Permission not found" });
        }

        return Ok(permission);
    }
}
