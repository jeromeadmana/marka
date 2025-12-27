using Marka.Api.Data;
using Marka.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Marka.Api.Services;

public class PermissionService : IPermissionService
{
    private readonly AppDbContext _context;

    public PermissionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UserHasPermissionAsync(Guid userId, string permissionCode)
    {
        var user = await _context.Users
            .Include(u => u.CustomRole)
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

        if (user == null)
        {
            return false;
        }

        // SuperAdmin has all permissions
        if (user.Role == UserRole.SuperAdmin)
        {
            return true;
        }

        // CustomerAdmin has all permissions except customer management
        if (user.Role == UserRole.CustomerAdmin)
        {
            // CustomerAdmin cannot manage customers (SuperAdmin only)
            if (permissionCode.StartsWith("Customers."))
            {
                return false;
            }
            return true;
        }

        // Regular users - check their custom role permissions
        if (user.CustomRoleId == null)
        {
            return false;
        }

        var hasPermission = await _context.RolePermissions
            .AnyAsync(rp =>
                rp.CustomRoleId == user.CustomRoleId &&
                rp.Permission.Code == permissionCode &&
                rp.Permission.IsActive);

        return hasPermission;
    }

    public async Task<List<string>> GetUserPermissionsAsync(Guid userId)
    {
        var user = await _context.Users
            .Include(u => u.CustomRole)
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

        if (user == null)
        {
            return new List<string>();
        }

        // SuperAdmin has all permissions
        if (user.Role == UserRole.SuperAdmin)
        {
            return await _context.Permissions
                .Where(p => p.IsActive)
                .Select(p => p.Code)
                .ToListAsync();
        }

        // CustomerAdmin has all permissions except customer management
        if (user.Role == UserRole.CustomerAdmin)
        {
            return await _context.Permissions
                .Where(p => p.IsActive && !p.Code.StartsWith("Customers."))
                .Select(p => p.Code)
                .ToListAsync();
        }

        // Regular users - get permissions from their custom role
        if (user.CustomRoleId == null)
        {
            return new List<string>();
        }

        var permissions = await _context.RolePermissions
            .Where(rp => rp.CustomRoleId == user.CustomRoleId && rp.Permission.IsActive)
            .Select(rp => rp.Permission.Code)
            .ToListAsync();

        return permissions;
    }
}
