using MediNexus.Api.Contracts.Locations;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public CitiesController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/cities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityResponse>>> GetAll()
        {
            var cities = await _db.Cities
                .AsNoTracking()
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .Select(c => new CityResponse
                {
                    Id = c.Id,
                    Code = c.Code,
                    FuripsCode = c.FuripsCode,
                    Name = c.Name,
                    StateId = c.StateId,
                    StateName = c.State.Name,
                    CountryId = c.State.CountryId,
                    CountryName = c.State.Country.Name
                })
                .ToListAsync();

            return Ok(cities);
        }

        // GET: api/cities/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CityResponse>> GetById(int id)
        {
            var city = await _db.Cities
                .AsNoTracking()
                .Where(c => c.IsActive && c.Id == id)
                .Select(c => new CityResponse
                {
                    Id = c.Id,
                    Code = c.Code,
                    FuripsCode = c.FuripsCode,
                    Name = c.Name,
                    StateId = c.StateId,
                    StateName = c.State.Name,
                    CountryId = c.State.CountryId,
                    CountryName = c.State.Country.Name
                })
                .FirstOrDefaultAsync();

            if (city is null)
                return NotFound("City not found.");

            return Ok(city);
        }
    }
}
