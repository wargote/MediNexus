using MediNexus.Api.Contracts.Insurer;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsurersController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public InsurersController(MediNexusDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get all insurers with optional filters.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InsurerResponse>>> GetAll(
            [FromQuery] int? countryId,
            [FromQuery] int? stateId,
            [FromQuery] int? cityId,
            [FromQuery] int? administratorTypeId,
            [FromQuery] string? search)
        {
            var query = _db.Insurers
                .AsNoTracking()
                .Where(i => i.IsActive)
                .Include(i => i.City)
                    .ThenInclude(c => c.State)
                        .ThenInclude(s => s.Country)
                .Include(i => i.AdministratorType)
                .AsQueryable();

            if (countryId.HasValue)
                query = query.Where(i => i.City.State.CountryId == countryId.Value);

            if (stateId.HasValue)
                query = query.Where(i => i.City.StateId == stateId.Value);

            if (cityId.HasValue)
                query = query.Where(i => i.CityId == cityId.Value);

            if (administratorTypeId.HasValue)
                query = query.Where(i => i.AdministratorTypeId == administratorTypeId.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(i =>
                    i.Name.Contains(term) ||
                    i.Nit.Contains(term) ||
                    (i.Code != null && i.Code.Contains(term)));
            }

            var insurers = await query
                .OrderBy(i => i.Id)
                .Select(i => new InsurerResponse
                {
                    Id = i.Id,
                    Name = i.Name,
                    Nit = i.Nit,
                    VerificationDigit = i.VerificationDigit,
                    Code = i.Code,
                    Address = i.Address,
                    Phone1 = i.Phone1,
                    Phone2 = i.Phone2,
                    Email = i.Email,

                    CityId = i.CityId,
                    CityName = i.City.Name,
                    StateId = i.City.StateId,
                    StateName = i.City.State.Name,
                    CountryId = i.City.State.CountryId,
                    CountryName = i.City.State.Country.Name,

                    AdministratorTypeId = i.AdministratorTypeId,
                    AdministratorTypeName = i.AdministratorType.Name,
                    AdministratorTypeShortName = i.AdministratorType.ShortName
                })
                .ToListAsync();

            return Ok(insurers);
        }

        // GET: api/insurers/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<InsurerResponse>> GetById(int id)
        {
            var insurer = await _db.Insurers
                .AsNoTracking()
                .Where(i => i.Id == id)
                .Include(i => i.City)
                    .ThenInclude(c => c.State)
                        .ThenInclude(s => s.Country)
                .Include(i => i.AdministratorType)
                .Select(i => new InsurerResponse
                {
                    Id = i.Id,
                    Name = i.Name,
                    Nit = i.Nit,
                    VerificationDigit = i.VerificationDigit,
                    Code = i.Code,
                    Address = i.Address,
                    Phone1 = i.Phone1,
                    Phone2 = i.Phone2,
                    Email = i.Email,

                    CityId = i.CityId,
                    CityName = i.City.Name,
                    StateId = i.City.StateId,
                    StateName = i.City.State.Name,
                    CountryId = i.City.State.CountryId,
                    CountryName = i.City.State.Country.Name,

                    AdministratorTypeId = i.AdministratorTypeId,
                    AdministratorTypeName = i.AdministratorType.Name,
                    AdministratorTypeShortName = i.AdministratorType.ShortName
                })
                .FirstOrDefaultAsync();

            if (insurer is null)
                return NotFound("Insurer not found.");

            return Ok(insurer);
        }

        // POST: api/insurers
        [HttpPost]
        public async Task<ActionResult<InsurerResponse>> Create([FromBody] InsurerCreateRequest req)
        {
            // ===== Basic validation =====
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // ===== Foreign key validation =====
            var cityExists = await _db.Cities.AnyAsync(c => c.Id == req.CityId && c.IsActive);
            if (!cityExists)
                return BadRequest("Invalid city.");

            var adminTypeExists = await _db.AdministratorTypes.AnyAsync(a => a.Id == req.AdministratorTypeId && a.IsActive);
            if (!adminTypeExists)
                return BadRequest("Invalid administrator type.");

            // ===== Uniqueness validation =====
            if (await _db.Insurers.AnyAsync(i => i.Nit == req.Nit))
                return Conflict("Nit already exists.");

            if (!string.IsNullOrWhiteSpace(req.Code) &&
                await _db.Insurers.AnyAsync(i => i.Code == req.Code))
                return Conflict("Code already exists.");

            var insurer = new Domain.Insurer.Insurer
            {
                Name = req.Name.Trim(),
                Nit = req.Nit.Trim(),
                VerificationDigit = req.VerificationDigit,
                Code = string.IsNullOrWhiteSpace(req.Code) ? null : req.Code.Trim(),
                Address = string.IsNullOrWhiteSpace(req.Address) ? null : req.Address.Trim(),
                CityId = req.CityId,
                Phone1 = string.IsNullOrWhiteSpace(req.Phone1) ? null : req.Phone1.Trim(),
                Phone2 = string.IsNullOrWhiteSpace(req.Phone2) ? null : req.Phone2.Trim(),
                Email = string.IsNullOrWhiteSpace(req.Email) ? null : req.Email.Trim(),
                AdministratorTypeId = req.AdministratorTypeId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _db.Insurers.Add(insurer);
            await _db.SaveChangesAsync();

            // Volvemos a leer con includes para armar el response completo
            var created = await _db.Insurers
                .AsNoTracking()
                .Where(i => i.Id == insurer.Id)
                .Include(i => i.City)
                    .ThenInclude(c => c.State)
                        .ThenInclude(s => s.Country)
                .Include(i => i.AdministratorType)
                .Select(i => new InsurerResponse
                {
                    Id = i.Id,
                    Name = i.Name,
                    Nit = i.Nit,
                    VerificationDigit = i.VerificationDigit,
                    Code = i.Code,
                    Address = i.Address,
                    Phone1 = i.Phone1,
                    Phone2 = i.Phone2,
                    Email = i.Email,

                    CityId = i.CityId,
                    CityName = i.City.Name,
                    StateId = i.City.StateId,
                    StateName = i.City.State.Name,
                    CountryId = i.City.State.CountryId,
                    CountryName = i.City.State.Country.Name,

                    AdministratorTypeId = i.AdministratorTypeId,
                    AdministratorTypeName = i.AdministratorType.Name,
                    AdministratorTypeShortName = i.AdministratorType.ShortName
                })
                .FirstAsync();

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/insurers/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<InsurerResponse>> Update(int id, [FromBody] InsurerUpdateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 1) Solo permitir actualizar si el registro está ACTIVO
            var insurer = await _db.Insurers
                .FirstOrDefaultAsync(i => i.Id == id && i.IsActive);

            if (insurer is null)
                return NotFound("Insurer not found or inactive.");

            // ===== Foreign key validation (ya estaban contra activos) =====
            var cityExists = await _db.Cities.AnyAsync(c => c.Id == req.CityId && c.IsActive);
            if (!cityExists)
                return BadRequest("Invalid city.");

            var adminTypeExists = await _db.AdministratorTypes.AnyAsync(a => a.Id == req.AdministratorTypeId && a.IsActive);
            if (!adminTypeExists)
                return BadRequest("Invalid administrator type.");

            // 2) Uniqueness validation SOLO contra aseguradoras ACTIVAS (ignorando el mismo insurer)
            if (await _db.Insurers.AnyAsync(i => i.IsActive && i.Id != id && i.Nit == req.Nit))
                return Conflict("Nit already exists.");

            if (!string.IsNullOrWhiteSpace(req.Code) &&
                await _db.Insurers.AnyAsync(i => i.IsActive && i.Id != id && i.Code == req.Code))
                return Conflict("Code already exists.");

            insurer.Name = req.Name.Trim();
            insurer.Nit = req.Nit.Trim();
            insurer.VerificationDigit = req.VerificationDigit;
            insurer.Code = string.IsNullOrWhiteSpace(req.Code) ? null : req.Code.Trim();
            insurer.Address = string.IsNullOrWhiteSpace(req.Address) ? null : req.Address.Trim();
            insurer.CityId = req.CityId;
            insurer.Phone1 = string.IsNullOrWhiteSpace(req.Phone1) ? null : req.Phone1.Trim();
            insurer.Phone2 = string.IsNullOrWhiteSpace(req.Phone2) ? null : req.Phone2.Trim();
            insurer.Email = string.IsNullOrWhiteSpace(req.Email) ? null : req.Email.Trim();
            insurer.AdministratorTypeId = req.AdministratorTypeId;
            insurer.IsActive = req.IsActive;
            insurer.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            // 3) Respuesta: si permites inactivar desde Update, NO filtres por IsActive
            // porque si req.IsActive = false, no lo vas a encontrar.
            var updated = await _db.Insurers
                .AsNoTracking()
                .Where(i => i.Id == insurer.Id) // sin i.IsActive para no romper cuando lo inactivas
                .Include(i => i.City)
                    .ThenInclude(c => c.State)
                        .ThenInclude(s => s.Country)
                .Include(i => i.AdministratorType)
                .Select(i => new InsurerResponse
                {
                    Id = i.Id,
                    Name = i.Name,
                    Nit = i.Nit,
                    VerificationDigit = i.VerificationDigit,
                    Code = i.Code,
                    Address = i.Address,
                    Phone1 = i.Phone1,
                    Phone2 = i.Phone2,
                    Email = i.Email,

                    CityId = i.CityId,
                    CityName = i.City.Name,
                    StateId = i.City.StateId,
                    StateName = i.City.State.Name,
                    CountryId = i.City.State.CountryId,
                    CountryName = i.City.State.Country.Name,

                    AdministratorTypeId = i.AdministratorTypeId,
                    AdministratorTypeName = i.AdministratorType.Name,
                    AdministratorTypeShortName = i.AdministratorType.ShortName
                })
                .FirstAsync();

            return Ok(updated);
        }

        // DELETE: api/insurers/5
        // Soft delete: IsActive = false
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var insurer = await _db.Insurers.FirstOrDefaultAsync(i => i.Id == id);
            if (insurer is null)
                return NotFound("Insurer not found.");

            if (!insurer.IsActive)
                return BadRequest("Insurer is already inactive.");

            insurer.IsActive = false;
            insurer.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
