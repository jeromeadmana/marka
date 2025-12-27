namespace Marka.Api.Models;

public class RolePermission
{
    public Guid Id { get; set; }
    public Guid CustomRoleId { get; set; }
    public Guid PermissionId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public CustomRole CustomRole { get; set; } = null!;
    public Permission Permission { get; set; } = null!;
}
