namespace Marka.Api.Models;

/// <summary>
/// Represents a type/kind/category of marka (e.g., "Fire Hydrant", "Street Sign", "Utility Pole")
/// Each customer can define their own marka contexts with specific attributes
/// </summary>
public class MarkaContext
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty; // Icon name or URL
    public string Description { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }

    // Display settings
    public string Color { get; set; } = "#3B82F6"; // Default blue color
    public int? DefaultRadius { get; set; } // Default radius in meters

    // Behavior settings
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    // Navigation properties
    public Customer Customer { get; set; } = null!;
    public User CreatedByUser { get; set; } = null!;
    public User? UpdatedByUser { get; set; }

    // Markas of this type
    public ICollection<MarkaEntity> Markas { get; set; } = new List<MarkaEntity>();

    // Attributes linked to this context
    public ICollection<MarkaContextAttribute> MarkaContextAttributes { get; set; } = new List<MarkaContextAttribute>();
}
