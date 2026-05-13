using MediNexus.Api.Contracts.TariffDetail;
using MediNexus.Domain.Tariffs;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TariffDetailsController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public TariffDetailsController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/tariffdetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TariffDetailResponse>>> GetAll([FromQuery] string? search)
        {
            var query = _db.TariffDetails
                .AsNoTracking()
                .Include(t => t.Tariff)
                .Include(t => t.SurgicalGroup)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = $"%{search.Trim()}%";
                query = query.Where(t => EF.Functions.ILike(t.Description, term));
            }

            var details = await query
                .OrderBy(t => t.Id)
                .Select(t => new TariffDetailResponse
                {
                    Id = t.Id,
                    ReferenceCode = t.ReferenceCode,
                    Description = t.Description,
                    Value = t.Value,
                    IsSurgicalProcedure = t.IsSurgicalProcedure,
                    Factors = t.Factors,
                    TariffId = t.TariffId,
                    TariffName = t.Tariff.Name,
                    SurgicalGroupId = t.SurgicalGroupId,
                    SurgicalGroupReferenceCode = t.SurgicalGroup.ReferenceCode
                })
                .ToListAsync();

            return Ok(details);
        }

        // GET: api/tariffdetails/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TariffDetailResponse>> GetById(int id)
        {
            var detail = await _db.TariffDetails
                .AsNoTracking()
                .Where(t => t.Id == id)
                .Include(t => t.Tariff)
                .Include(t => t.SurgicalGroup)
                .Select(t => new TariffDetailResponse
                {
                    Id = t.Id,
                    ReferenceCode = t.ReferenceCode,
                    Description = t.Description,
                    Value = t.Value,
                    IsSurgicalProcedure = t.IsSurgicalProcedure,
                    Factors = t.Factors,
                    TariffId = t.TariffId,
                    TariffName = t.Tariff.Name,
                    SurgicalGroupId = t.SurgicalGroupId,
                    SurgicalGroupReferenceCode = t.SurgicalGroup.ReferenceCode
                })
                .FirstOrDefaultAsync();

            if (detail is null)
                return NotFound("Tariff detail not found.");

            return Ok(detail);
        }

        // POST: api/tariffdetails
        [HttpPost]
        public async Task<ActionResult<TariffDetailResponse>> Create([FromBody] TariffDetailCreateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _db.Tariffs.AnyAsync(t => t.Id == req.TariffId))
                return BadRequest("Invalid tariff.");

            if (!await _db.SurgicalGroups.AnyAsync(s => s.Id == req.SurgicalGroupId))
                return BadRequest("Invalid surgical group.");

            var detail = new TariffDetail
            {
                ReferenceCode = req.ReferenceCode,
                Description = req.Description.Trim(),
                Value = req.Value,
                IsSurgicalProcedure = req.IsSurgicalProcedure,
                Factors = req.Factors,
                TariffId = req.TariffId,
                SurgicalGroupId = req.SurgicalGroupId
            };

            _db.TariffDetails.Add(detail);
            await _db.SaveChangesAsync();

            var createdAction = await GetById(detail.Id);
            var createdResult = createdAction.Result as OkObjectResult;
            return CreatedAtAction(nameof(GetById), new { id = detail.Id }, createdResult?.Value);
        }

        // PUT: api/tariffdetails/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<TariffDetailResponse>> Update(int id, [FromBody] TariffDetailUpdateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var detail = await _db.TariffDetails.FirstOrDefaultAsync(t => t.Id == id);
            if (detail is null)
                return NotFound("Tariff detail not found.");

            if (!await _db.Tariffs.AnyAsync(t => t.Id == req.TariffId))
                return BadRequest("Invalid tariff.");

            if (!await _db.SurgicalGroups.AnyAsync(s => s.Id == req.SurgicalGroupId))
                return BadRequest("Invalid surgical group.");

            detail.ReferenceCode = req.ReferenceCode;
            detail.Description = req.Description.Trim();
            detail.Value = req.Value;
            detail.IsSurgicalProcedure = req.IsSurgicalProcedure;
            detail.Factors = req.Factors;
            detail.TariffId = req.TariffId;
            detail.SurgicalGroupId = req.SurgicalGroupId;

            await _db.SaveChangesAsync();

            var updatedAction = await GetById(detail.Id);
            var updatedResult = updatedAction.Result as OkObjectResult;
            return Ok(updatedResult?.Value);
        }

        // DELETE: api/tariffdetails/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var detail = await _db.TariffDetails.FirstOrDefaultAsync(t => t.Id == id);
            if (detail is null)
                return NotFound("Tariff detail not found.");

            _db.TariffDetails.Remove(detail);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
