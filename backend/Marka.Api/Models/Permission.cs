namespace Marka.Api.Models;

public class Permission
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty; // e.g., "Access.Web", "Marka.Create"
    public string Name { get; set; } = string.Empty; // e.g., "Access Web Application"
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // e.g., "Access", "Marka", "Assignment"
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

// Permission Categories and Codes
public static class PermissionCodes
{
    // Platform Access
    public const string AccessWeb = "Access.Web";
    public const string AccessMobile = "Access.Mobile";

    // Marka Management
    public const string MarkaView = "Marka.View";
    public const string MarkaCreate = "Marka.Create";
    public const string MarkaEdit = "Marka.Edit";
    public const string MarkaDelete = "Marka.Delete";

    // Assignment Management (future feature)
    public const string AssignmentView = "Assignment.View";
    public const string AssignmentCreate = "Assignment.Create";
    public const string AssignmentAssign = "Assignment.Assign";
    public const string AssignmentComplete = "Assignment.Complete";

    // Report Access (future feature)
    public const string ReportsView = "Reports.View";
    public const string ReportsExport = "Reports.Export";

    // User Management
    public const string UsersView = "Users.View";
    public const string UsersCreate = "Users.Create";
    public const string UsersEdit = "Users.Edit";
    public const string UsersDelete = "Users.Delete";

    // Role Management
    public const string RolesView = "Roles.View";
    public const string RolesCreate = "Roles.Create";
    public const string RolesEdit = "Roles.Edit";
    public const string RolesDelete = "Roles.Delete";

    // Customer Management (SuperAdmin only)
    public const string CustomersView = "Customers.View";
    public const string CustomersCreate = "Customers.Create";
    public const string CustomersEdit = "Customers.Edit";
    public const string CustomersDelete = "Customers.Delete";
}
