using MediNexus.Api.Contracts.Tariff;
using MediNexus.Domain.Tariffs;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TariffsController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public TariffsController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/tariffs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TariffResponse>>> GetAll([FromQuery] string? search)
        {
            var query = _db.Tariffs
                .AsNoTracking()
                .Where(t => t.IsActive)
                .Include(t => t.ValueMethod)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = $"%{search.Trim()}%";
                query = query.Where(t => EF.Functions.ILike(t.Name, term));
            }

            var tariffs = await query
                .OrderBy(t => t.Id)
                .Select(t => new TariffResponse
                {
                    Id = t.Id,
                    Name = t.Name,
                    ValueMethodId = t.ValueMethodId,
                    ValueMethodDescription = t.ValueMethod.Description,
                    IsActive = t.IsActive,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                })
                .ToListAsync();

            return Ok(tariffs);
        }

        // GET: api/tariffs/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TariffResponse>> GetById(int id)
        {
            var tariff = await _db.Tariffs
                .AsNoTracking()
                .Where(t => t.Id == id)
                .Include(t => t.ValueMethod)
                .Select(t => new TariffResponse
                {
                    Id = t.Id,
                    Name = t.Name,
                    ValueMethodId = t.ValueMethodId,
                    ValueMethodDescription = t.ValueMethod.Description,
                    IsActive = t.IsActive,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (tariff is null)
                return NotFound("Tariff not found.");

            return Ok(tariff);
        }

        // POST: api/tariffs
        [HttpPost]
        public async Task<ActionResult<TariffResponse>> Create([FromBody] TariffCreateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _db.ValueMethods.AnyAsync(v => v.Id == req.ValueMethodId))
                return BadRequest("Invalid value method.");

            var tariff = new Tariff
            {
                Name = req.Name.Trim(),
                ValueMethodId = req.ValueMethodId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Tariffs.Add(tariff);
            await _db.SaveChangesAsync();

            var createdAction = await GetById(tariff.Id);
            var createdResult = createdAction.Result as OkObjectResult;
            return CreatedAtAction(nameof(GetById), new { id = tariff.Id }, createdResult?.Value);
        }

        // PUT: api/tariffs/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<TariffResponse>> Update(int id, [FromBody] TariffUpdateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tariff = await _db.Tariffs.FirstOrDefaultAsync(t => t.Id == id);
            if (tariff is null)
                return NotFound("Tariff not found.");

            if (!await _db.ValueMethods.AnyAsync(v => v.Id == req.ValueMethodId))
                return BadRequest("Invalid value method.");

            tariff.Name = req.Name.Trim();
            tariff.ValueMethodId = req.ValueMethodId;
            tariff.IsActive = req.IsActive;
            tariff.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            var updatedAction = await GetById(tariff.Id);
            var updatedResult = updatedAction.Result as OkObjectResult;
            return Ok(updatedResult?.Value);
        }

        // DELETE: api/tariffs/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tariff = await _db.Tariffs.FirstOrDefaultAsync(t => t.Id == id && t.IsActive);
            if (tariff is null)
                return NotFound("Tariff not found.");

            tariff.IsActive = false;
            tariff.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
