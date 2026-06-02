using MediNexus.Api.Contracts.TariffDetail;
using MediNexus.Domain.Tariffs;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SurgicalGroupsController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public SurgicalGroupsController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/surgicalgroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SurgicalGroupResponse>>> GetAll([FromQuery] string? search)
        {
            var query = _db.SurgicalGroups
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = $"%{search.Trim()}%";
                query = query.Where(g => EF.Functions.ILike(g.ReferenceCode, term));
            }

            var groups = await query
                .OrderBy(g => g.Id)
                .Select(g => new SurgicalGroupResponse
                {
                    Id = g.Id,
                    ReferenceCode = g.ReferenceCode
                })
                .ToListAsync();

            return Ok(groups);
        }

        // GET: api/surgicalgroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SurgicalGroupResponse>> GetById(int id)
        {
            var group = await _db.SurgicalGroups
                .AsNoTracking()
                .Where(g => g.Id == id)
                .Select(g => new SurgicalGroupResponse
                {
                    Id = g.Id,
                    ReferenceCode = g.ReferenceCode
                })
                .FirstOrDefaultAsync();

            if (group == null) return NotFound();

            return Ok(group);
        }

        // POST: api/surgicalgroups
        [HttpPost]
        public async Task<ActionResult<SurgicalGroupResponse>> Create([FromBody] SurgicalGroupCreateRequest req)
        {
            var exists = await _db.SurgicalGroups
                .AnyAsync(g => g.ReferenceCode.ToLower() == req.ReferenceCode.ToLower());
            
            if (exists)
                return BadRequest("Ya existe un grupo quirúrgico con este código de referencia.");

            var group = new SurgicalGroup
            {
                ReferenceCode = req.ReferenceCode
            };

            _db.SurgicalGroups.Add(group);
            await _db.SaveChangesAsync();

            var response = new SurgicalGroupResponse
            {
                Id = group.Id,
                ReferenceCode = group.ReferenceCode
            };

            return CreatedAtAction(nameof(GetById), new { id = group.Id }, response);
        }

        // PUT: api/surgicalgroups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SurgicalGroupUpdateRequest req)
        {
            var group = await _db.SurgicalGroups.FindAsync(id);
            if (group == null) return NotFound();

            var exists = await _db.SurgicalGroups
                .AnyAsync(g => g.Id != id && g.ReferenceCode.ToLower() == req.ReferenceCode.ToLower());

            if (exists)
                return BadRequest("Ya existe un grupo quirúrgico con este código de referencia.");

            group.ReferenceCode = req.ReferenceCode;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/surgicalgroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var group = await _db.SurgicalGroups.FindAsync(id);
            if (group == null) return NotFound();

            // Verify if it is in use
            var hasDetails = await _db.TariffDetails.AnyAsync(t => t.SurgicalGroupId == id);
            if (hasDetails)
                return BadRequest("No se puede eliminar el grupo quirúrgico porque tiene detalles de tarifa asociados.");

            _db.SurgicalGroups.Remove(group);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
