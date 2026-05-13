using MediNexus.Api.Contracts.TariffDetail;
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
    }
}
