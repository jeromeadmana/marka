namespace Marka.Api.Models;

public class MarkaAttribute
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // text, number, date, dropdown, boolean, file
    public bool Required { get; set; } = false;
    public string? Options { get; set; } // JSON for dropdown options
    public string? ValidationRules { get; set; } // JSON for validation rules
    public int DisplayOrder { get; set; } = 0;

    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
