using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Marka.Api.Data;
using Marka.Api.Models;
using System.Security.Claims;

namespace Marka.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AttributesController : ControllerBase
{
    private readonly AppDbContext _context;

    public AttributesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/attributes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetAttributes()
    {
        try
        {
            var userCustomerId = User.FindFirst("CustomerId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var query = _context.Attributes
                .Include(a => a.Customer)
                .Include(a => a.CreatedByUser)
                .AsQueryable();

            // Filter by customer for non-SuperAdmin users
            if (userRole != "SuperAdmin" && !string.IsNullOrEmpty(userCustomerId))
            {
                var customerId = Guid.Parse(userCustomerId);
                query = query.Where(a => a.CustomerId == customerId);
            }

            var attributes = await query
                .OrderBy(a => a.Name)
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Label,
                    a.Type,
                    a.DefaultValue,
                    a.Options,
                    a.Required,
                    a.ReadOnly,
                    a.Persist,
                    a.IsSystem,
                    a.DisplayOrder,
                    a.CustomerId,
                    CustomerName = a.Customer != null ? a.Customer.Name : null,
                    a.IsActive,
                    a.CreatedAt,
                    a.UpdatedAt,
                    CreatedBy = a.CreatedByUser != null ? a.CreatedByUser.Email : null
                })
                .ToListAsync();

            return Ok(attributes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving attributes", error = ex.Message });
        }
    }

    // GET: api/attributes/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetAttribute(Guid id)
    {
        try
        {
            var userCustomerId = User.FindFirst("CustomerId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var attribute = await _context.Attributes
                .Include(a => a.Customer)
                .Include(a => a.CreatedByUser)
                .Include(a => a.UpdatedByUser)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attribute == null)
            {
                return NotFound(new { message = "Attribute not found" });
            }

            // Check customer isolation
            if (userRole != "SuperAdmin" && !string.IsNullOrEmpty(userCustomerId))
            {
                var customerId = Guid.Parse(userCustomerId);
                if (attribute.CustomerId != customerId)
                {
                    return Forbid();
                }
            }

            var result = new
            {
                attribute.Id,
                attribute.Name,
                attribute.Label,
                attribute.Type,
                attribute.DefaultValue,
                attribute.Options,
                attribute.ValidationRules,
                attribute.Required,
                attribute.ReadOnly,
                attribute.Persist,
                attribute.IsSystem,
                attribute.DisplayOrder,
                attribute.CustomerId,
                CustomerName = attribute.Customer != null ? attribute.Customer.Name : null,
                attribute.IsActive,
                attribute.CreatedAt,
                attribute.UpdatedAt,
                CreatedBy = attribute.CreatedByUser?.Email,
                UpdatedBy = attribute.UpdatedByUser?.Email
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving attribute", error = ex.Message });
        }
    }

    // POST: api/attributes
    [HttpPost]
    public async Task<ActionResult<object>> CreateAttribute([FromBody] CreateAttributeRequest request)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdClaim);
            var userCustomerId = User.FindFirst("CustomerId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Determine which customer this attribute belongs to
            Guid targetCustomerId;
            if (userRole == "SuperAdmin" && request.CustomerId.HasValue)
            {
                targetCustomerId = request.CustomerId.Value;
            }
            else if (!string.IsNullOrEmpty(userCustomerId))
            {
                targetCustomerId = Guid.Parse(userCustomerId);
            }
            else
            {
                return BadRequest(new { message = "Unable to determine customer" });
            }

            var attribute = new MarkaAttribute
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Label = request.Label,
                Type = request.Type,
                DefaultValue = request.DefaultValue,
                Options = request.Options,
                ValidationRules = request.ValidationRules,
                Required = request.Required ?? false,
                ReadOnly = request.ReadOnly ?? false,
                Persist = request.Persist ?? true,
                IsSystem = false, // User-created attributes are not system attributes
                DisplayOrder = request.DisplayOrder ?? 0,
                CustomerId = targetCustomerId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userId
            };

            _context.Attributes.Add(attribute);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAttribute), new { id = attribute.Id }, new
            {
                attribute.Id,
                attribute.Name,
                attribute.Label,
                attribute.Type,
                attribute.DefaultValue,
                attribute.Options,
                attribute.Required,
                attribute.ReadOnly,
                attribute.Persist,
                attribute.DisplayOrder,
                attribute.CustomerId,
                attribute.IsActive,
                attribute.CreatedAt
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error creating attribute", error = ex.Message });
        }
    }

    // PUT: api/attributes/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAttribute(Guid id, [FromBody] UpdateAttributeRequest request)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdClaim);
            var userCustomerId = User.FindFirst("CustomerId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var attribute = await _context.Attributes.FindAsync(id);
            if (attribute == null)
            {
                return NotFound(new { message = "Attribute not found" });
            }

            // Check customer isolation
            if (userRole != "SuperAdmin" && !string.IsNullOrEmpty(userCustomerId))
            {
                var customerId = Guid.Parse(userCustomerId);
                if (attribute.CustomerId != customerId)
                {
                    return Forbid();
                }
            }

            // Don't allow updating system attributes
            if (attribute.IsSystem)
            {
                return BadRequest(new { message = "Cannot update system attributes" });
            }

            attribute.Name = request.Name;
            attribute.Label = request.Label;
            attribute.Type = request.Type;
            attribute.DefaultValue = request.DefaultValue;
            attribute.Options = request.Options;
            attribute.ValidationRules = request.ValidationRules;
            attribute.Required = request.Required ?? attribute.Required;
            attribute.ReadOnly = request.ReadOnly ?? attribute.ReadOnly;
            attribute.Persist = request.Persist ?? attribute.Persist;
            attribute.DisplayOrder = request.DisplayOrder ?? attribute.DisplayOrder;
            attribute.IsActive = request.IsActive ?? attribute.IsActive;
            attribute.UpdatedAt = DateTime.UtcNow;
            attribute.UpdatedBy = userId;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error updating attribute", error = ex.Message });
        }
    }

    // DELETE: api/attributes/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttribute(Guid id)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdClaim);
            var userCustomerId = User.FindFirst("CustomerId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var attribute = await _context.Attributes.FindAsync(id);
            if (attribute == null)
            {
                return NotFound(new { message = "Attribute not found" });
            }

            // Check customer isolation
            if (userRole != "SuperAdmin" && !string.IsNullOrEmpty(userCustomerId))
            {
                var customerId = Guid.Parse(userCustomerId);
                if (attribute.CustomerId != customerId)
                {
                    return Forbid();
                }
            }

            // Don't allow deleting system attributes
            if (attribute.IsSystem)
            {
                return BadRequest(new { message = "Cannot delete system attributes" });
            }

            // Soft delete
            attribute.IsDeleted = true;
            attribute.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error deleting attribute", error = ex.Message });
        }
    }
}

// DTOs
public class CreateAttributeRequest
{
    public string Name { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // text, number, date, dropdown, boolean, file
    public string? DefaultValue { get; set; }
    public string? Options { get; set; } // JSON for dropdown options
    public string? ValidationRules { get; set; } // JSON for validation rules
    public bool? Required { get; set; }
    public bool? ReadOnly { get; set; }
    public bool? Persist { get; set; }
    public int? DisplayOrder { get; set; }
    public Guid? CustomerId { get; set; } // Only for SuperAdmin
}

public class UpdateAttributeRequest
{
    public string Name { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? DefaultValue { get; set; }
    public string? Options { get; set; }
    public string? ValidationRules { get; set; }
    public bool? Required { get; set; }
    public bool? ReadOnly { get; set; }
    public bool? Persist { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsActive { get; set; }
}
