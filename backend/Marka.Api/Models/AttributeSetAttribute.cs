namespace Marka.Api.Models;

/// <summary>
/// Junction table linking AttributeSet to MarkaAttributes
/// Defines which attributes are in each attribute set and their order
/// </summary>
public class AttributeSetAttribute
{
    public Guid Id { get; set; }
    public Guid AttributeSetId { get; set; }
    public Guid MarkaAttributeId { get; set; }
    public int AttributeOrder { get; set; } // Display order of this attribute in the set

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public AttributeSet AttributeSet { get; set; } = null!;
    public MarkaAttribute MarkaAttribute { get; set; } = null!;
}
