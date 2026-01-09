using MediNexus.Api.Contracts.Administrator;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers
{
    [ApiController]
    [Route("api/administrator-types")]
    public class AdministratorTypesController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public AdministratorTypesController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/administrator-types
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdministratorTypeResponse>>> GetAll()
        {
            var types = await _db.AdministratorTypes
                .AsNoTracking()
                .Where(t => t.IsActive)
                .OrderBy(t => t.Name)
                .Select(t => new AdministratorTypeResponse
                {
                    Id = t.Id,
                    Name = t.Name,
                    ShortName = t.ShortName
                })
                .ToListAsync();

            return Ok(types);
        }
    }
}
