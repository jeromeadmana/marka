namespace Marka.Api.Models;

/// <summary>
/// Junction table linking MarkaContext to MarkaAttributes
/// Defines which attributes are available for each marka type and their order
/// </summary>
public class MarkaContextAttribute
{
    public Guid Id { get; set; }
    public Guid MarkaContextId { get; set; }
    public Guid MarkaAttributeId { get; set; }
    public int AttributeOrder { get; set; } // Display order of this attribute
    public bool IsRequired { get; set; } = false;
    public bool IsReadOnly { get; set; } = false;
    public bool IsFeatured { get; set; } = false; // Show in list view
    public int? FeaturedOrder { get; set; } // Order in featured list

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public MarkaContext MarkaContext { get; set; } = null!;
    public MarkaAttribute MarkaAttribute { get; set; } = null!;
}
