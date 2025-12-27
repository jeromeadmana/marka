using Marka.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Marka.Api.Data;

public static class SeedPermissions
{
    public static async Task Initialize(AppDbContext context)
    {
        // Check if permissions already exist
        if (await context.Permissions.AnyAsync())
        {
            return; // Permissions already seeded
        }

        var permissions = new List<Permission>
        {
            // Platform Access Permissions
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.AccessWeb,
                Name = "Access Web Application",
                Description = "Allows user to access the web application",
                Category = "Access",
                IsActive = true
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.AccessMobile,
                Name = "Access Mobile Application",
                Description = "Allows user to access the mobile application",
                Category = "Access",
                IsActive = true
            },

            // Marka Permissions
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.MarkaView,
                Name = "View Markas",
                Description = "Allows user to view markas",
                Category = "Marka",
                IsActive = true
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.MarkaCreate,
                Name = "Create Markas",
                Description = "Allows user to create new markas",
                Category = "Marka",
                IsActive = true
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.MarkaEdit,
                Name = "Edit Markas",
                Description = "Allows user to edit existing markas",
                Category = "Marka",
                IsActive = true
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.MarkaDelete,
                Name = "Delete Markas",
                Description = "Allows user to delete markas",
                Category = "Marka",
                IsActive = true
            },

            // Assignment Permissions
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.AssignmentView,
                Name = "View Assignments",
                Description = "Allows user to view assignments",
                Category = "Assignment",
                IsActive = true
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.AssignmentCreate,
                Name = "Create Assignments",
                Description = "Allows user to create new assignments",
                Category = "Assignment",
                IsActive = true
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.AssignmentAssign,
                Name = "Assign Assignments",
                Description = "Allows user to assign assignments to users",
                Category = "Assignment",
                IsActive = true
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.AssignmentComplete,
                Name = "Complete Assignments",
                Description = "Allows user to mark assignments as complete",
                Category = "Assignment",
                IsActive = true
            },

            // Report Permissions
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.ReportsView,
                Name = "View Reports",
                Description = "Allows user to view reports",
                Category = "Reports",
                IsActive = true
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.ReportsExport,
                Name = "Export Reports",
                Description = "Allows user to export reports",
                Category = "Reports",
                IsActive = true
            },

            // User Management Permissions
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.UsersView,
                Name = "View Users",
                Description = "Allows user to view other users",
                Category = "Users",
                IsActive = true
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.UsersCreate,
                Name = "Create Users",
                Description = "Allows user to create new users",
                Category = "Users",
                IsActive = true
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.UsersEdit,
                Name = "Edit Users",
                Description = "Allows user to edit existing users",
                Category = "Users",
                IsActive = true
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.UsersDelete,
                Name = "Delete Users",
                Description = "Allows user to delete users",
                Category = "Users",
                IsActive = true
            },

            // Role Management Permissions
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.RolesView,
                Name = "View Roles",
                Description = "Allows user to view roles",
                Category = "Roles",
                IsActive = true
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.RolesCreate,
                Name = "Create Roles",
                Description = "Allows user to create new roles",
                Category = "Roles",
                IsActive = true
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.RolesEdit,
                Name = "Edit Roles",
                Description = "Allows user to edit existing roles",
                Category = "Roles",
                IsActive = true
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.RolesDelete,
                Name = "Delete Roles",
                Description = "Allows user to delete roles",
                Category = "Roles",
                IsActive = true
            },

            // Customer Management Permissions (SuperAdmin only)
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.CustomersView,
                Name = "View Customers",
                Description = "Allows user to view customers (SuperAdmin only)",
                Category = "Customers",
                IsActive = true
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.CustomersCreate,
                Name = "Create Customers",
                Description = "Allows user to create new customers (SuperAdmin only)",
                Category = "Customers",
                IsActive = true
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.CustomersEdit,
                Name = "Edit Customers",
                Description = "Allows user to edit existing customers (SuperAdmin only)",
                Category = "Customers",
                IsActive = true
            },
            new Permission
            {
                Id = Guid.NewGuid(),
                Code = PermissionCodes.CustomersDelete,
                Name = "Delete Customers",
                Description = "Allows user to delete customers (SuperAdmin only)",
                Category = "Customers",
                IsActive = true
            }
        };

        await context.Permissions.AddRangeAsync(permissions);
        await context.SaveChangesAsync();

        Console.WriteLine($"Seeded {permissions.Count} permissions");
    }
}
