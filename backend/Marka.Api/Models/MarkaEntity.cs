namespace Marka.Api.Models;

public class MarkaEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Simple lat/long coordinates (no PostGIS needed)
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public string? Address { get; set; }
    public string? Category { get; set; }
    public string Status { get; set; } = "Active"; // Active, Inactive, Pending, Archived

    // Marka Type/Context
    public Guid? MarkaContextId { get; set; }
    public MarkaContext? MarkaContext { get; set; }

    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public Guid CreatedByUserId { get; set; }
    public User? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; } // Soft delete

    // Navigation properties
    public ICollection<AttributeValue> AttributeValues { get; set; } = new List<AttributeValue>();
}
