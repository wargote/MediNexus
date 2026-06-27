using MediNexus.Api.Contracts.HCInicial;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediNexus.Api.Controllers.HCInicial
{
    [Route("api/cie10codes")]
    [ApiController]
    [Authorize]
    public class Cie10CodesController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public Cie10CodesController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/cie10codes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cie10CodeResponse>>> GetAll()
        {
            var registros = await _db.Cie10Codes
                .AsNoTracking()
                .Where(c => c.IsActive)
                .OrderBy(c => c.Codigo)
                .Select(c => MapToResponse(c))
                .ToListAsync();

            return Ok(registros);
        }

        // GET: api/cie10codes/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Cie10CodeResponse>> GetById(int id)
        {
            var registro = await _db.Cie10Codes
                .AsNoTracking()
                .Where(c => c.Id == id && c.IsActive)
                .Select(c => MapToResponse(c))
                .FirstOrDefaultAsync();

            if (registro is null)
                return NotFound("Código CIE-10 no encontrado.");

            return Ok(registro);
        }

        // GET: api/cie10codes/buscar?q=hipertension
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<Cie10CodeResponse>>> Buscar([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("Ingrese un término de búsqueda.");

            var term = q.Trim().ToLower();

            var registros = await _db.Cie10Codes
                .AsNoTracking()
                .Where(c => c.IsActive &&
                    (c.Codigo.ToLower().Contains(term) || c.Descripcion.ToLower().Contains(term)))
                .OrderBy(c => c.Codigo)
                .Take(50)
                .Select(c => MapToResponse(c))
                .ToListAsync();

            return Ok(registros);
        }

        private static Cie10CodeResponse MapToResponse(Domain.HCInicial.Cie10Code c) => new()
        {
            Id = c.Id,
            Codigo = c.Codigo,
            Descripcion = c.Descripcion,
            IsActive = c.IsActive,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        };
    }
}
