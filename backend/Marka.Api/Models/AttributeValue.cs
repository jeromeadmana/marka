namespace Marka.Api.Models;

public class AttributeValue
{
    public Guid Id { get; set; }
    public Guid AttributeId { get; set; }
    public MarkaAttribute? Attribute { get; set; }

    public Guid MarkaId { get; set; }
    public MarkaEntity? Marka { get; set; }

    public string? Value { get; set; } // Store as text, convert based on Attribute.Type

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
