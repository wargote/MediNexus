using MediNexus.Api.Contracts.Locations;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatesController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public StatesController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/states
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StateResponse>>> GetAll()
        {
            var states = await _db.States
                .AsNoTracking()
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .Select(s => new StateResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    DaneCode = s.DaneCode,
                    CountryId = s.CountryId,
                    CountryName = s.Country.Name
                })
                .ToListAsync();

            return Ok(states);
        }

        // GET: api/states/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<StateResponse>> GetById(int id)
        {
            var state = await _db.States
                .AsNoTracking()
                .Where(s => s.IsActive && s.Id == id)
                .Select(s => new StateResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    DaneCode = s.DaneCode,
                    CountryId = s.CountryId,
                    CountryName = s.Country.Name
                })
                .FirstOrDefaultAsync();

            if (state is null)
                return NotFound("State not found.");

            return Ok(state);
        }

        // GET: api/states/5/cities
        [HttpGet("{id:int}/cities")]
        public async Task<ActionResult<IEnumerable<CityResponse>>> GetCitiesByState(int id)
        {
            var exists = await _db.States.AnyAsync(s => s.Id == id && s.IsActive);
            if (!exists)
                return NotFound("State not found.");

            var cities = await _db.Cities
                .AsNoTracking()
                .Where(c => c.IsActive && c.StateId == id)
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
    }
}
