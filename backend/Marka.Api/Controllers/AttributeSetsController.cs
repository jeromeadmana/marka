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
public class AttributeSetsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AttributeSetsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/attributesets
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetAttributeSets()
    {
        try
        {
            var userCustomerId = User.FindFirst("CustomerId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var query = _context.AttributeSets
                .Include(aset => aset.Customer)
                .Include(aset => aset.CreatedByUser)
                .Include(aset => aset.AttributeSetAttributes)
                    .ThenInclude(asa => asa.MarkaAttribute)
                .AsQueryable();

            // Filter by customer for non-SuperAdmin users
            if (userRole != "SuperAdmin" && !string.IsNullOrEmpty(userCustomerId))
            {
                var customerId = Guid.Parse(userCustomerId);
                query = query.Where(aset => aset.CustomerId == customerId);
            }

            var sets = await query
                .OrderBy(aset => aset.Name)
                .Select(aset => new
                {
                    aset.Id,
                    aset.Name,
                    aset.Description,
                    aset.CustomerId,
                    CustomerName = aset.Customer.Name,
                    aset.IsActive,
                    aset.CreatedAt,
                    aset.UpdatedAt,
                    CreatedBy = aset.CreatedByUser.Email,
                    AttributeCount = aset.AttributeSetAttributes.Count
                })
                .ToListAsync();

            return Ok(sets);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving attribute sets", error = ex.Message });
        }
    }

    // GET: api/attributesets/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetAttributeSet(Guid id)
    {
        try
        {
            var userCustomerId = User.FindFirst("CustomerId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var attributeSet = await _context.AttributeSets
                .Include(aset => aset.Customer)
                .Include(aset => aset.CreatedByUser)
                .Include(aset => aset.UpdatedByUser)
                .Include(aset => aset.AttributeSetAttributes.OrderBy(asa => asa.AttributeOrder))
                    .ThenInclude(asa => asa.MarkaAttribute)
                .FirstOrDefaultAsync(aset => aset.Id == id);

            if (attributeSet == null)
            {
                return NotFound(new { message = "Attribute set not found" });
            }

            // Check customer isolation
            if (userRole != "SuperAdmin" && !string.IsNullOrEmpty(userCustomerId))
            {
                var customerId = Guid.Parse(userCustomerId);
                if (attributeSet.CustomerId != customerId)
                {
                    return Forbid();
                }
            }

            var result = new
            {
                attributeSet.Id,
                attributeSet.Name,
                attributeSet.Description,
                attributeSet.CustomerId,
                CustomerName = attributeSet.Customer.Name,
                attributeSet.IsActive,
                attributeSet.CreatedAt,
                attributeSet.UpdatedAt,
                CreatedBy = attributeSet.CreatedByUser.Email,
                UpdatedBy = attributeSet.UpdatedByUser?.Email,
                Attributes = attributeSet.AttributeSetAttributes.Select(asa => new
                {
                    asa.Id,
                    asa.MarkaAttributeId,
                    AttributeName = asa.MarkaAttribute.Name,
                    AttributeLabel = asa.MarkaAttribute.Label,
                    AttributeType = asa.MarkaAttribute.Type,
                    asa.AttributeOrder
                }).ToList()
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving attribute set", error = ex.Message });
        }
    }

    // POST: api/attributesets
    [HttpPost]
    public async Task<ActionResult<object>> CreateAttributeSet([FromBody] CreateAttributeSetRequest request)
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

            // Determine which customer this set belongs to
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

            var attributeSet = new AttributeSet
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description ?? string.Empty,
                CustomerId = targetCustomerId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userId
            };

            _context.AttributeSets.Add(attributeSet);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAttributeSet), new { id = attributeSet.Id }, new
            {
                attributeSet.Id,
                attributeSet.Name,
                attributeSet.Description,
                attributeSet.CustomerId,
                attributeSet.IsActive,
                attributeSet.CreatedAt
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error creating attribute set", error = ex.Message });
        }
    }

    // PUT: api/attributesets/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAttributeSet(Guid id, [FromBody] UpdateAttributeSetRequest request)
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

            var attributeSet = await _context.AttributeSets.FindAsync(id);
            if (attributeSet == null)
            {
                return NotFound(new { message = "Attribute set not found" });
            }

            // Check customer isolation
            if (userRole != "SuperAdmin" && !string.IsNullOrEmpty(userCustomerId))
            {
                var customerId = Guid.Parse(userCustomerId);
                if (attributeSet.CustomerId != customerId)
                {
                    return Forbid();
                }
            }

            attributeSet.Name = request.Name;
            attributeSet.Description = request.Description ?? attributeSet.Description;
            attributeSet.IsActive = request.IsActive ?? attributeSet.IsActive;
            attributeSet.UpdatedAt = DateTime.UtcNow;
            attributeSet.UpdatedBy = userId;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error updating attribute set", error = ex.Message });
        }
    }

    // DELETE: api/attributesets/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttributeSet(Guid id)
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

            var attributeSet = await _context.AttributeSets.FindAsync(id);
            if (attributeSet == null)
            {
                return NotFound(new { message = "Attribute set not found" });
            }

            // Check customer isolation
            if (userRole != "SuperAdmin" && !string.IsNullOrEmpty(userCustomerId))
            {
                var customerId = Guid.Parse(userCustomerId);
                if (attributeSet.CustomerId != customerId)
                {
                    return Forbid();
                }
            }

            // Soft delete
            attributeSet.IsDeleted = true;
            attributeSet.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error deleting attribute set", error = ex.Message });
        }
    }

    // POST: api/attributesets/{id}/attributes
    [HttpPost("{id}/attributes")]
    public async Task<ActionResult<object>> AddAttributeToSet(Guid id, [FromBody] AddAttributeToSetRequest request)
    {
        try
        {
            var userCustomerId = User.FindFirst("CustomerId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var attributeSet = await _context.AttributeSets.FindAsync(id);
            if (attributeSet == null)
            {
                return NotFound(new { message = "Attribute set not found" });
            }

            // Check customer isolation
            if (userRole != "SuperAdmin" && !string.IsNullOrEmpty(userCustomerId))
            {
                var customerId = Guid.Parse(userCustomerId);
                if (attributeSet.CustomerId != customerId)
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
            if (attribute.CustomerId != attributeSet.CustomerId)
            {
                return BadRequest(new { message = "Attribute does not belong to the same customer" });
            }

            // Check if already linked
            var existingLink = await _context.AttributeSetAttributes
                .FirstOrDefaultAsync(asa => asa.AttributeSetId == id && asa.MarkaAttributeId == request.MarkaAttributeId);

            if (existingLink != null)
            {
                return Conflict(new { message = "Attribute already in this set" });
            }

            var attributeSetAttribute = new AttributeSetAttribute
            {
                Id = Guid.NewGuid(),
                AttributeSetId = id,
                MarkaAttributeId = request.MarkaAttributeId,
                AttributeOrder = request.AttributeOrder,
                CreatedAt = DateTime.UtcNow
            };

            _context.AttributeSetAttributes.Add(attributeSetAttribute);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                attributeSetAttribute.Id,
                attributeSetAttribute.AttributeSetId,
                attributeSetAttribute.MarkaAttributeId,
                attributeSetAttribute.AttributeOrder
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error adding attribute to set", error = ex.Message });
        }
    }

    // DELETE: api/attributesets/{id}/attributes/{attributeId}
    [HttpDelete("{id}/attributes/{attributeId}")]
    public async Task<IActionResult> RemoveAttributeFromSet(Guid id, Guid attributeId)
    {
        try
        {
            var userCustomerId = User.FindFirst("CustomerId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var attributeSet = await _context.AttributeSets.FindAsync(id);
            if (attributeSet == null)
            {
                return NotFound(new { message = "Attribute set not found" });
            }

            // Check customer isolation
            if (userRole != "SuperAdmin" && !string.IsNullOrEmpty(userCustomerId))
            {
                var customerId = Guid.Parse(userCustomerId);
                if (attributeSet.CustomerId != customerId)
                {
                    return Forbid();
                }
            }

            var link = await _context.AttributeSetAttributes
                .FirstOrDefaultAsync(asa => asa.AttributeSetId == id && asa.MarkaAttributeId == attributeId);

            if (link == null)
            {
                return NotFound(new { message = "Attribute not in this set" });
            }

            _context.AttributeSetAttributes.Remove(link);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error removing attribute from set", error = ex.Message });
        }
    }
}

// DTOs
public class CreateAttributeSetRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? CustomerId { get; set; } // Only for SuperAdmin
}

public class UpdateAttributeSetRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}

public class AddAttributeToSetRequest
{
    public Guid MarkaAttributeId { get; set; }
    public int AttributeOrder { get; set; }
}
