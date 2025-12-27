namespace Marka.Api.Models;

public class CustomRole
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty; // e.g., "Field Agent", "Dispatcher", "hello1"
    public string Description { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Customer Customer { get; set; } = null!;
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
