using Marka.Api.Authorization;
using Marka.Api.Data;
using Marka.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Marka.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MarkasController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<MarkasController> _logger;

    public MarkasController(AppDbContext context, ILogger<MarkasController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/markas
    [HttpGet]
    [RequirePermission(PermissionCodes.MarkaView)]
    public async Task<ActionResult<IEnumerable<MarkaEntity>>> GetMarkas()
    {
        try
        {
            var userCustomerId = User.FindFirst("CustomerId")?.Value;

            var query = _context.Markas.AsQueryable();

            // Filter by customer for non-SuperAdmin users
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "SuperAdmin" && !string.IsNullOrEmpty(userCustomerId))
            {
                var customerId = Guid.Parse(userCustomerId);
                query = query.Where(m => m.CustomerId == customerId);
            }

            var markas = await query
                .Include(m => m.Customer)
                .Include(m => m.CreatedBy)
                .Include(m => m.AttributeValues)
                    .ThenInclude(av => av.Attribute)
                .ToListAsync();

            return Ok(markas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching markas");
            return StatusCode(500, "Error fetching markas");
        }
    }

    // GET: api/markas/{id}
    [HttpGet("{id}")]
    [RequirePermission(PermissionCodes.MarkaView)]
    public async Task<ActionResult<MarkaEntity>> GetMarka(Guid id)
    {
        try
        {
            var marka = await _context.Markas
                .Include(m => m.Customer)
                .Include(m => m.CreatedBy)
                .Include(m => m.AttributeValues)
                    .ThenInclude(av => av.Attribute)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (marka == null)
            {
                return NotFound($"Marka with ID {id} not found");
            }

            return Ok(marka);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching marka {MarkaId}", id);
            return StatusCode(500, "Error fetching marka");
        }
    }

    // POST: api/markas
    [HttpPost]
    [RequirePermission(PermissionCodes.MarkaCreate)]
    public async Task<ActionResult<MarkaEntity>> CreateMarka(CreateMarkaDto dto)
    {
        try
        {
            var marka = new MarkaEntity
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Address = dto.Address,
                Category = dto.Category,
                Status = dto.Status ?? "Active",
                CustomerId = dto.CustomerId,
                CreatedByUserId = dto.CreatedByUserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Markas.Add(marka);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created marka {MarkaId} - {MarkaName}", marka.Id, marka.Name);

            return CreatedAtAction(nameof(GetMarka), new { id = marka.Id }, marka);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating marka");
            return StatusCode(500, "Error creating marka");
        }
    }

    // PUT: api/markas/{id}
    [HttpPut("{id}")]
    [RequirePermission(PermissionCodes.MarkaEdit)]
    public async Task<IActionResult> UpdateMarka(Guid id, UpdateMarkaDto dto)
    {
        try
        {
            var marka = await _context.Markas.FindAsync(id);
            if (marka == null)
            {
                return NotFound($"Marka with ID {id} not found");
            }

            // Update fields
            marka.Name = dto.Name ?? marka.Name;
            marka.Description = dto.Description ?? marka.Description;
            marka.Latitude = dto.Latitude ?? marka.Latitude;
            marka.Longitude = dto.Longitude ?? marka.Longitude;
            marka.Address = dto.Address ?? marka.Address;
            marka.Category = dto.Category ?? marka.Category;
            marka.Status = dto.Status ?? marka.Status;
            marka.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated marka {MarkaId}", id);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating marka {MarkaId}", id);
            return StatusCode(500, "Error updating marka");
        }
    }

    // DELETE: api/markas/{id}
    [HttpDelete("{id}")]
    [RequirePermission(PermissionCodes.MarkaDelete)]
    public async Task<IActionResult> DeleteMarka(Guid id)
    {
        try
        {
            var marka = await _context.Markas.FindAsync(id);
            if (marka == null)
            {
                return NotFound($"Marka with ID {id} not found");
            }

            // Soft delete
            marka.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Soft deleted marka {MarkaId}", id);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting marka {MarkaId}", id);
            return StatusCode(500, "Error deleting marka");
        }
    }
}

// DTOs (Data Transfer Objects)
public record CreateMarkaDto(
    string Name,
    string? Description,
    double Latitude,
    double Longitude,
    string? Address,
    string? Category,
    string? Status,
    Guid CustomerId,
    Guid CreatedByUserId
);

public record UpdateMarkaDto(
    string? Name,
    string? Description,
    double? Latitude,
    double? Longitude,
    string? Address,
    string? Category,
    string? Status
);
