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
public class MarkaContextsController : ControllerBase
{
    private readonly AppDbContext _context;

    public MarkaContextsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/markacontexts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetMarkaContexts()
    {
        try
        {
            var userCustomerId = User.FindFirst("CustomerId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var query = _context.MarkaContexts
                .Include(mc => mc.Customer)
                .Include(mc => mc.CreatedByUser)
                .Include(mc => mc.MarkaContextAttributes)
                    .ThenInclude(mca => mca.MarkaAttribute)
                .AsQueryable();

            // Filter by customer for non-SuperAdmin users
            if (userRole != "SuperAdmin" && !string.IsNullOrEmpty(userCustomerId))
            {
                var customerId = Guid.Parse(userCustomerId);
                query = query.Where(mc => mc.CustomerId == customerId);
            }

            var contexts = await query
                .OrderBy(mc => mc.Name)
                .Select(mc => new
                {
                    mc.Id,
                    mc.Name,
                    mc.Icon,
                    mc.Description,
                    mc.Color,
                    mc.DefaultRadius,
                    mc.CustomerId,
                    CustomerName = mc.Customer.Name,
                    mc.IsActive,
                    mc.CreatedAt,
                    mc.UpdatedAt,
                    CreatedBy = mc.CreatedByUser.Email,
                    AttributeCount = mc.MarkaContextAttributes.Count,
                    MarkaCount = mc.Markas.Count
                })
                .ToListAsync();

            return Ok(contexts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving marka contexts", error = ex.Message });
        }
    }

    // GET: api/markacontexts/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetMarkaContext(Guid id)
    {
        try
        {
            var userCustomerId = User.FindFirst("CustomerId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var context = await _context.MarkaContexts
                .Include(mc => mc.Customer)
                .Include(mc => mc.CreatedByUser)
                .Include(mc => mc.UpdatedByUser)
                .Include(mc => mc.MarkaContextAttributes.OrderBy(mca => mca.AttributeOrder))
                    .ThenInclude(mca => mca.MarkaAttribute)
                .FirstOrDefaultAsync(mc => mc.Id == id);

            if (context == null)
            {
                return NotFound(new { message = "Marka context not found" });
            }

            // Check customer isolation
            if (userRole != "SuperAdmin" && !string.IsNullOrEmpty(userCustomerId))
            {
                var customerId = Guid.Parse(userCustomerId);
                if (context.CustomerId != customerId)
                {
                    return Forbid();
                }
            }

            var result = new
            {
                context.Id,
                context.Name,
                context.Icon,
                context.Description,
                context.Color,
                context.DefaultRadius,
                context.CustomerId,
                CustomerName = context.Customer.Name,
                context.IsActive,
                context.CreatedAt,
                context.UpdatedAt,
                CreatedBy = context.CreatedByUser.Email,
                UpdatedBy = context.UpdatedByUser?.Email,
                Attributes = context.MarkaContextAttributes.Select(mca => new
                {
                    mca.Id,
                    mca.MarkaAttributeId,
                    AttributeName = mca.MarkaAttribute.Name,
                    AttributeLabel = mca.MarkaAttribute.Label,
                    AttributeType = mca.MarkaAttribute.Type,
                    mca.AttributeOrder,
                    mca.IsRequired,
                    mca.IsReadOnly,
                    mca.IsFeatured,
                    mca.FeaturedOrder
                }).ToList(),
                MarkaCount = context.Markas.Count
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving marka context", error = ex.Message });
        }
    }

    // POST: api/markacontexts
    [HttpPost]
    public async Task<ActionResult<object>> CreateMarkaContext([FromBody] CreateMarkaContextRequest request)
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

            // Determine which customer this context belongs to
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

            var markaContext = new MarkaContext
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Icon = request.Icon ?? string.Empty,
                Description = request.Description ?? string.Empty,
                Color = request.Color ?? "#3B82F6",
                DefaultRadius = request.DefaultRadius,
                CustomerId = targetCustomerId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userId
            };

            _context.MarkaContexts.Add(markaContext);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMarkaContext), new { id = markaContext.Id }, new
            {
                markaContext.Id,
                markaContext.Name,
                markaContext.Icon,
                markaContext.Description,
                markaContext.Color,
                markaContext.DefaultRadius,
                markaContext.CustomerId,
                markaContext.IsActive,
                markaContext.CreatedAt
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error creating marka context", error = ex.Message });
        }
    }

    // PUT: api/markacontexts/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMarkaContext(Guid id, [FromBody] UpdateMarkaContextRequest request)
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

            var markaContext = await _context.MarkaContexts.FindAsync(id);
            if (markaContext == null)
            {
                return NotFound(new { message = "Marka context not found" });
            }

            // Check customer isolation
            if (userRole != "SuperAdmin" && !string.IsNullOrEmpty(userCustomerId))
            {
                var customerId = Guid.Parse(userCustomerId);
                if (markaContext.CustomerId != customerId)
                {
                    return Forbid();
                }
            }

            markaContext.Name = request.Name;
            markaContext.Icon = request.Icon ?? markaContext.Icon;
            markaContext.Description = request.Description ?? markaContext.Description;
            markaContext.Color = request.Color ?? markaContext.Color;
            markaContext.DefaultRadius = request.DefaultRadius ?? markaContext.DefaultRadius;
            markaContext.IsActive = request.IsActive ?? markaContext.IsActive;
            markaContext.UpdatedAt = DateTime.UtcNow;
            markaContext.UpdatedBy = userId;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error updating marka context", error = ex.Message });
        }
    }

    // DELETE: api/markacontexts/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMarkaContext(Guid id)
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

            var markaContext = await _context.MarkaContexts.FindAsync(id);
            if (markaContext == null)
            {
                return NotFound(new { message = "Marka context not found" });
            }

            // Check customer isolation
            if (userRole != "SuperAdmin" && !string.IsNullOrEmpty(userCustomerId))
            {
                var customerId = Guid.Parse(userCustomerId);
                if (markaContext.CustomerId != customerId)
                {
                    return Forbid();
                }
            }

            // Soft delete
            markaContext.IsDeleted = true;
            markaContext.DeletedAt = DateTime.UtcNow;
            markaContext.DeletedBy = userId;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error deleting marka context", error = ex.Message });
        }
    }

    // POST: api/markacontexts/{id}/attributes
    [HttpPost("{id}/attributes")]
    public async Task<ActionResult<object>> AddAttributeToContext(Guid id, [FromBody] AddAttributeToContextRequest request)
    {
        try
        {
            var userCustomerId = User.FindFirst("CustomerId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var markaContext = await _context.MarkaContexts.FindAsync(id);
            if (markaContext == null)
            {
                return NotFound(new { message = "Marka context not found" });
            }

            // Check customer isolation
            if (userRole != "SuperAdmin" && !string.IsNullOrEmpty(userCustomerId))
            {
                var customerId = Guid.Parse(userCustomerId);
                if (markaContext.CustomerId != customerId)
                {
                    return Forbid();
                }
            }

            var attribute = await _context.Attributes.FindAsync(request.MarkaAttributeId);
            if (attribute == null)
            {
                return NotFound(new { message = "Attribute not found" });
            }

            // Ensure attribute belongs to same customer
            if (attribute.CustomerId != markaContext.CustomerId)
            {
                return BadRequest(new { message = "Attribute does not belong to the same customer" });
            }

            // Check if already linked
            var existingLink = await _context.MarkaContextAttributes
                .FirstOrDefaultAsync(mca => mca.MarkaContextId == id && mca.MarkaAttributeId == request.MarkaAttributeId);

            if (existingLink != null)
            {
                return Conflict(new { message = "Attribute already linked to this context" });
            }

            var markaContextAttribute = new MarkaContextAttribute
            {
                Id = Guid.NewGuid(),
                MarkaContextId = id,
                MarkaAttributeId = request.MarkaAttributeId,
                AttributeOrder = request.AttributeOrder,
                IsRequired = request.IsRequired ?? false,
                IsReadOnly = request.IsReadOnly ?? false,
                IsFeatured = request.IsFeatured ?? false,
                FeaturedOrder = request.FeaturedOrder,
                CreatedAt = DateTime.UtcNow
            };

            _context.MarkaContextAttributes.Add(markaContextAttribute);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                markaContextAttribute.Id,
                markaContextAttribute.MarkaContextId,
                markaContextAttribute.MarkaAttributeId,
                markaContextAttribute.AttributeOrder,
                markaContextAttribute.IsRequired,
                markaContextAttribute.IsReadOnly,
                markaContextAttribute.IsFeatured,
                markaContextAttribute.FeaturedOrder
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error adding attribute to context", error = ex.Message });
        }
    }

    // DELETE: api/markacontexts/{id}/attributes/{attributeId}
    [HttpDelete("{id}/attributes/{attributeId}")]
    public async Task<IActionResult> RemoveAttributeFromContext(Guid id, Guid attributeId)
    {
        try
        {
            var userCustomerId = User.FindFirst("CustomerId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var markaContext = await _context.MarkaContexts.FindAsync(id);
            if (markaContext == null)
            {
                return NotFound(new { message = "Marka context not found" });
            }

            // Check customer isolation
            if (userRole != "SuperAdmin" && !string.IsNullOrEmpty(userCustomerId))
            {
                var customerId = Guid.Parse(userCustomerId);
                if (markaContext.CustomerId != customerId)
                {
                    return Forbid();
                }
            }

            var link = await _context.MarkaContextAttributes
                .FirstOrDefaultAsync(mca => mca.MarkaContextId == id && mca.MarkaAttributeId == attributeId);

            if (link == null)
            {
                return NotFound(new { message = "Attribute not linked to this context" });
            }

            _context.MarkaContextAttributes.Remove(link);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error removing attribute from context", error = ex.Message });
        }
    }
}

// DTOs
public class CreateMarkaContextRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
    public int? DefaultRadius { get; set; }
    public Guid? CustomerId { get; set; } // Only for SuperAdmin
}

public class UpdateMarkaContextRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
    public int? DefaultRadius { get; set; }
    public bool? IsActive { get; set; }
}

public class AddAttributeToContextRequest
{
    public Guid MarkaAttributeId { get; set; }
    public int AttributeOrder { get; set; }
    public bool? IsRequired { get; set; }
    public bool? IsReadOnly { get; set; }
    public bool? IsFeatured { get; set; }
    public int? FeaturedOrder { get; set; }
}
