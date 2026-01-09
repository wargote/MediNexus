using MediNexus.Api.Contracts.Locations;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public CountriesController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryResponse>>> GetAll()
        {
            var countries = await _db.Countries
                .AsNoTracking()
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .Select(c => new CountryResponse
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name
                })
                .ToListAsync();

            return Ok(countries);
        }

        // GET: api/countries/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CountryResponse>> GetById(int id)
        {
            var country = await _db.Countries
                .AsNoTracking()
                .Where(c => c.IsActive && c.Id == id)
                .Select(c => new CountryResponse
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name
                })
                .FirstOrDefaultAsync();

            if (country is null)
                return NotFound("Country not found.");

            return Ok(country);
        }

        // GET: api/countries/5/states
        [HttpGet("{id:int}/states")]
        public async Task<ActionResult<IEnumerable<StateResponse>>> GetStatesByCountry(int id)
        {
            var exists = await _db.Countries.AnyAsync(c => c.Id == id && c.IsActive);
            if (!exists)
                return NotFound("Country not found.");

            var states = await _db.States
                .AsNoTracking()
                .Where(s => s.IsActive && s.CountryId == id)
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
    }
}
