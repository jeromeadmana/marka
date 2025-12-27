namespace Marka.Api.Models;

/// <summary>
/// Represents a predefined collection of attributes that can be reused across multiple marka contexts
/// Example: "Standard Utility Pole Attributes", "Fire Hydrant Inspection Fields"
/// </summary>
public class AttributeSet
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    // Navigation properties
    public Customer Customer { get; set; } = null!;
    public User CreatedByUser { get; set; } = null!;
    public User? UpdatedByUser { get; set; }

    // Attributes in this set
    public ICollection<AttributeSetAttribute> AttributeSetAttributes { get; set; } = new List<AttributeSetAttribute>();
}
