using MediNexus.Api.Contracts.Medicines;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers.Medicines
{
    [Route("api/medicines/catalogs")]
    [ApiController]
    [Authorize]
    public class MedicinesCatalogsController : ControllerBase
    {
        private readonly MediNexusDbContext _context;

        public MedicinesCatalogsController(MediNexusDbContext context)
        {
            _context = context;
        }

        // GET: api/medicines/catalogs/element-types
        [HttpGet("element-types")]
        [ProducesResponseType(typeof(IEnumerable<ElementTypeResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetElementTypes()
        {
            var items = await _context.ElementTypes
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new ElementTypeResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Ok(items);
        }

        // GET: api/medicines/catalogs/element-usages
        [HttpGet("element-usages")]
        [ProducesResponseType(typeof(IEnumerable<ElementUsageResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetElementUsages()
        {
            var items = await _context.ElementUsages
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new ElementUsageResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Ok(items);
        }
    }
}
