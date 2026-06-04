using MediNexus.Api.Contracts.Medicines;
using MediNexus.Domain.Medicines;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers.Medicines
{
    [ApiController]
    [Route("api/medical-devices")]
    [Authorize]
    public class MedicalDevicesController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public MedicalDevicesController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/medical-devices
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MedicalDeviceResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MedicalDeviceResponse>>> GetAll([FromQuery] string? search = null)
        {
            var query = _db.MedicalDevices
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Include(x => x.ElementType)
                .Include(x => x.ElementUsage)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = $"%{search.Trim()}%";
                query = query.Where(x =>
                    EF.Functions.ILike(x.ElementName, term) ||
                    EF.Functions.ILike(x.RipsCode, term) ||
                    EF.Functions.ILike(x.ElementType.Name, term) ||
                    EF.Functions.ILike(x.ElementUsage.Name, term));
            }

            var items = await query
                .OrderBy(x => x.ElementName)
                .Select(x => new MedicalDeviceResponse
                {
                    Id = x.Id,
                    ElementName = x.ElementName,
                    ElementTypeId = x.ElementTypeId,
                    ElementTypeName = x.ElementType.Name,
                    ElementUsageId = x.ElementUsageId,
                    ElementUsageName = x.ElementUsage.Name,
                    RipsCode = x.RipsCode,
                    IsReusable = x.IsReusable,
                    IsInvasive = x.IsInvasive,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync();

            return Ok(items);
        }

        // GET: api/medical-devices/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(MedicalDeviceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MedicalDeviceResponse>> GetById(int id)
        {
            var item = await BuildMedicalDeviceResponse(id);

            if (item is null)
                return NotFound("Medical device not found.");

            return Ok(item);
        }

        // POST: api/medical-devices
        [HttpPost]
        [ProducesResponseType(typeof(MedicalDeviceResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MedicalDeviceResponse>> Create([FromBody] MedicalDeviceCreateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(req.ElementName))
                return BadRequest("Element name is required.");

            if (string.IsNullOrWhiteSpace(req.RipsCode))
                return BadRequest("RIPS code is required.");

            var foreignKeysAreValid = await ValidateForeignKeys(req.ElementTypeId, req.ElementUsageId);
            if (!foreignKeysAreValid.IsValid)
                return BadRequest(foreignKeysAreValid.Message);

            var medicalDevice = new MedicalDevice
            {
                ElementName = req.ElementName.Trim(),
                ElementTypeId = req.ElementTypeId,
                ElementUsageId = req.ElementUsageId,
                RipsCode = req.RipsCode.Trim(),
                IsReusable = req.IsReusable,
                IsInvasive = req.IsInvasive,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.MedicalDevices.Add(medicalDevice);
            await _db.SaveChangesAsync();

            var created = await BuildMedicalDeviceResponse(medicalDevice.Id);

            return CreatedAtAction(nameof(GetById), new { id = medicalDevice.Id }, created);
        }

        // PUT: api/medical-devices/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(MedicalDeviceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MedicalDeviceResponse>> Update(int id, [FromBody] MedicalDeviceUpdateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(req.ElementName))
                return BadRequest("Element name is required.");

            if (string.IsNullOrWhiteSpace(req.RipsCode))
                return BadRequest("RIPS code is required.");

            var medicalDevice = await _db.MedicalDevices.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
            if (medicalDevice is null)
                return NotFound("Medical device not found or inactive.");

            var foreignKeysAreValid = await ValidateForeignKeys(req.ElementTypeId, req.ElementUsageId);
            if (!foreignKeysAreValid.IsValid)
                return BadRequest(foreignKeysAreValid.Message);

            medicalDevice.ElementName = req.ElementName.Trim();
            medicalDevice.ElementTypeId = req.ElementTypeId;
            medicalDevice.ElementUsageId = req.ElementUsageId;
            medicalDevice.RipsCode = req.RipsCode.Trim();
            medicalDevice.IsReusable = req.IsReusable;
            medicalDevice.IsInvasive = req.IsInvasive;
            medicalDevice.IsActive = req.IsActive;
            medicalDevice.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            var updated = await BuildMedicalDeviceResponse(medicalDevice.Id);

            return Ok(updated);
        }

        // DELETE: api/medical-devices/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var medicalDevice = await _db.MedicalDevices.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
            if (medicalDevice is null)
                return NotFound("Medical device not found.");

            medicalDevice.IsActive = false;
            medicalDevice.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        private async Task<(bool IsValid, string Message)> ValidateForeignKeys(int elementTypeId, int elementUsageId)
        {
            if (!await _db.ElementTypes.AnyAsync(x => x.Id == elementTypeId && x.IsActive))
                return (false, "Invalid element type.");

            if (!await _db.ElementUsages.AnyAsync(x => x.Id == elementUsageId && x.IsActive))
                return (false, "Invalid element usage.");

            return (true, string.Empty);
        }

        private async Task<MedicalDeviceResponse?> BuildMedicalDeviceResponse(int id)
        {
            return await _db.MedicalDevices
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Include(x => x.ElementType)
                .Include(x => x.ElementUsage)
                .Select(x => new MedicalDeviceResponse
                {
                    Id = x.Id,
                    ElementName = x.ElementName,
                    ElementTypeId = x.ElementTypeId,
                    ElementTypeName = x.ElementType.Name,
                    ElementUsageId = x.ElementUsageId,
                    ElementUsageName = x.ElementUsage.Name,
                    RipsCode = x.RipsCode,
                    IsReusable = x.IsReusable,
                    IsInvasive = x.IsInvasive,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .FirstOrDefaultAsync();
        }
    }
}
