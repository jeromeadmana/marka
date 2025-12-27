namespace Marka.Api.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User; // SuperAdmin, CustomerAdmin, or User (with custom role)
    public Guid? CustomRoleId { get; set; } // Null for SuperAdmin/CustomerAdmin, set for Users with custom roles
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public CustomRole? CustomRole { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
}
