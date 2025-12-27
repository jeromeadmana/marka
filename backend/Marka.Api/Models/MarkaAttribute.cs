namespace Marka.Api.Models;

public class MarkaAttribute
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // text, number, date, dropdown, boolean, file
    public string? DefaultValue { get; set; }
    public string? Options { get; set; } // JSON for dropdown options
    public string? ValidationRules { get; set; } // JSON for validation rules

    // Behavior settings
    public bool Required { get; set; } = false;
    public bool ReadOnly { get; set; } = false;
    public bool Persist { get; set; } = true; // Should value persist across updates
    public bool IsSystem { get; set; } = false; // System-defined attribute

    public int DisplayOrder { get; set; } = 0;

    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    // Navigation properties
    public User? CreatedByUser { get; set; }
    public User? UpdatedByUser { get; set; }

    // Relationships
    public ICollection<MarkaContextAttribute> MarkaContextAttributes { get; set; } = new List<MarkaContextAttribute>();
    public ICollection<AttributeSetAttribute> AttributeSetAttributes { get; set; } = new List<AttributeSetAttribute>();
}
