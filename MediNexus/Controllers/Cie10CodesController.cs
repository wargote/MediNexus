using MediNexus.Api.Contracts.Diagnoses;
using MediNexus.Domain.Diagnoses;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Cie10CodesController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public Cie10CodesController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/cie10codes
        /// <summary>
        /// Returns all CIE-10 codes. Optionally filter by code or description keyword.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cie10CodeResponse>>> GetAll([FromQuery] string? search)
        {
            var query = _db.Cie10Codes
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = $"%{search.Trim()}%";
                query = query.Where(c =>
                    EF.Functions.ILike(c.Cod4, term) ||
                    EF.Functions.ILike(c.DescripcionCodigoCuatroCaracteres, term));
            }

            var result = await query
                .OrderBy(c => c.Cod4)
                .Select(c => new Cie10CodeResponse
                {
                    Cod4 = c.Cod4,
                    DescripcionCodigoCuatroCaracteres = c.DescripcionCodigoCuatroCaracteres
                })
                .ToListAsync();

            return Ok(result);
        }

        // GET: api/cie10codes/A001
        /// <summary>
        /// Returns a single CIE-10 code by its COD_4 key.
        /// </summary>
        [HttpGet("{cod4}")]
        public async Task<ActionResult<Cie10CodeResponse>> GetById(string cod4)
        {
            var code = await _db.Cie10Codes
                .AsNoTracking()
                .Where(c => c.Cod4 == cod4.ToUpper())
                .Select(c => new Cie10CodeResponse
                {
                    Cod4 = c.Cod4,
                    DescripcionCodigoCuatroCaracteres = c.DescripcionCodigoCuatroCaracteres
                })
                .FirstOrDefaultAsync();

            if (code == null) return NotFound();

            return Ok(code);
        }

        // POST: api/cie10codes
        /// <summary>
        /// Creates a new CIE-10 code entry.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Cie10CodeResponse>> Create([FromBody] Cie10CodeCreateRequest req)
        {
            var normalizedCode = req.Cod4.Trim().ToUpper();

            var exists = await _db.Cie10Codes
                .AnyAsync(c => c.Cod4 == normalizedCode);

            if (exists)
                return BadRequest($"Ya existe un código CIE-10 con el código '{normalizedCode}'.");

            var entity = new Cie10Code
            {
                Cod4 = normalizedCode,
                DescripcionCodigoCuatroCaracteres = req.DescripcionCodigoCuatroCaracteres.Trim()
            };

            _db.Cie10Codes.Add(entity);
            await _db.SaveChangesAsync();

            var response = new Cie10CodeResponse
            {
                Cod4 = entity.Cod4,
                DescripcionCodigoCuatroCaracteres = entity.DescripcionCodigoCuatroCaracteres
            };

            return CreatedAtAction(nameof(GetById), new { cod4 = entity.Cod4 }, response);
        }

        // PUT: api/cie10codes/A001
        /// <summary>
        /// Updates the description of an existing CIE-10 code. The COD_4 key cannot be changed.
        /// </summary>
        [HttpPut("{cod4}")]
        public async Task<IActionResult> Update(string cod4, [FromBody] Cie10CodeUpdateRequest req)
        {
            var normalizedCode = cod4.Trim().ToUpper();

            var entity = await _db.Cie10Codes.FindAsync(normalizedCode);
            if (entity == null) return NotFound();

            entity.DescripcionCodigoCuatroCaracteres = req.DescripcionCodigoCuatroCaracteres.Trim();

            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/cie10codes/A001
        /// <summary>
        /// Deletes a CIE-10 code entry by its COD_4 key.
        /// </summary>
        [HttpDelete("{cod4}")]
        public async Task<IActionResult> Delete(string cod4)
        {
            var normalizedCode = cod4.Trim().ToUpper();

            var entity = await _db.Cie10Codes.FindAsync(normalizedCode);
            if (entity == null) return NotFound();

            _db.Cie10Codes.Remove(entity);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
