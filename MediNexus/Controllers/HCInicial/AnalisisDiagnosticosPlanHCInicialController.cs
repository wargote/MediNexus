using MediNexus.Api.Contracts.HCInicial;
using MediNexus.Domain.HCInicial;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediNexus.Api.Controllers.HCInicial
{
    [Route("api/analisis-diagnosticos-plan-hcinicial")]
    [ApiController]
    [Authorize]
    public class AnalisisDiagnosticosPlanHCInicialController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public AnalisisDiagnosticosPlanHCInicialController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/analisis-diagnosticos-plan-hcinicial
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnalisisDiagnosticosPlanHCInicialResponse>>> GetAll()
        {
            var registros = await _db.AnalisisDiagnosticosPlanHCInicial
                .AsNoTracking()
                .Where(a => a.IsActive)
                .Include(a => a.Diagnosticos).ThenInclude(d => d.Cie10Code)
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => MapToResponse(a))
                .ToListAsync();

            return Ok(registros);
        }

        // GET: api/analisis-diagnosticos-plan-hcinicial/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AnalisisDiagnosticosPlanHCInicialResponse>> GetById(int id)
        {
            var registro = await _db.AnalisisDiagnosticosPlanHCInicial
                .AsNoTracking()
                .Where(a => a.Id == id && a.IsActive)
                .Include(a => a.Diagnosticos).ThenInclude(d => d.Cie10Code)
                .FirstOrDefaultAsync();

            if (registro is null)
                return NotFound("Registro de análisis, diagnósticos y plan no encontrado.");

            return Ok(MapToResponse(registro));
        }

        // POST: api/analisis-diagnosticos-plan-hcinicial
        [HttpPost]
        public async Task<ActionResult<AnalisisDiagnosticosPlanHCInicialResponse>> Create([FromBody] AnalisisDiagnosticosPlanHCInicialCreateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (req.IdsCie10.Count > 0)
            {
                var cie10Validos = await _db.Cie10Codes
                    .Where(c => req.IdsCie10.Contains(c.Id) && c.IsActive)
                    .Select(c => c.Id)
                    .ToListAsync();

                var invalidos = req.IdsCie10.Except(cie10Validos).ToList();
                if (invalidos.Count > 0)
                    return NotFound($"Los siguientes IDs CIE-10 no existen o están inactivos: {string.Join(", ", invalidos)}");
            }

            var registro = new AnalisisDiagnosticosPlanHCInicial
            {
                Analisis = req.Analisis?.Trim(),
                Plan = req.Plan?.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.AnalisisDiagnosticosPlanHCInicial.Add(registro);
            await _db.SaveChangesAsync();

            foreach (var idCie10 in req.IdsCie10.Distinct())
            {
                _db.AnalisisDiagnosticosPlanHCInicialDiagnosticos.Add(new AnalisisDiagnosticosPlanHCInicialDiagnostico
                {
                    IdAnalisisDiagnosticosPlan = registro.Id,
                    IdCie10 = idCie10
                });
            }

            await _db.SaveChangesAsync();

            await _db.Entry(registro).Collection(a => a.Diagnosticos).Query()
                .Include(d => d.Cie10Code).LoadAsync();

            return CreatedAtAction(nameof(GetById), new { id = registro.Id }, MapToResponse(registro));
        }

        // PUT: api/analisis-diagnosticos-plan-hcinicial/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<AnalisisDiagnosticosPlanHCInicialResponse>> Update(int id, [FromBody] AnalisisDiagnosticosPlanHCInicialUpdateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registro = await _db.AnalisisDiagnosticosPlanHCInicial
                .Include(a => a.Diagnosticos).ThenInclude(d => d.Cie10Code)
                .FirstOrDefaultAsync(a => a.Id == id && a.IsActive);

            if (registro is null)
                return NotFound("Registro de análisis, diagnósticos y plan no encontrado.");

            if (req.IdsCie10.Count > 0)
            {
                var cie10Validos = await _db.Cie10Codes
                    .Where(c => req.IdsCie10.Contains(c.Id) && c.IsActive)
                    .Select(c => c.Id)
                    .ToListAsync();

                var invalidos = req.IdsCie10.Except(cie10Validos).ToList();
                if (invalidos.Count > 0)
                    return NotFound($"Los siguientes IDs CIE-10 no existen o están inactivos: {string.Join(", ", invalidos)}");
            }

            registro.Analisis = req.Analisis?.Trim();
            registro.Plan = req.Plan?.Trim();
            registro.IsActive = req.IsActive;
            registro.UpdatedAt = DateTime.UtcNow;

            _db.AnalisisDiagnosticosPlanHCInicialDiagnosticos.RemoveRange(registro.Diagnosticos);

            foreach (var idCie10 in req.IdsCie10.Distinct())
            {
                _db.AnalisisDiagnosticosPlanHCInicialDiagnosticos.Add(new AnalisisDiagnosticosPlanHCInicialDiagnostico
                {
                    IdAnalisisDiagnosticosPlan = registro.Id,
                    IdCie10 = idCie10
                });
            }

            await _db.SaveChangesAsync();

            await _db.Entry(registro).Collection(a => a.Diagnosticos).Query()
                .Include(d => d.Cie10Code).LoadAsync();

            return Ok(MapToResponse(registro));
        }

        // DELETE: api/analisis-diagnosticos-plan-hcinicial/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var registro = await _db.AnalisisDiagnosticosPlanHCInicial
                .FirstOrDefaultAsync(a => a.Id == id && a.IsActive);

            if (registro is null)
                return NotFound("Registro de análisis, diagnósticos y plan no encontrado.");

            registro.IsActive = false;
            registro.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        private static AnalisisDiagnosticosPlanHCInicialResponse MapToResponse(AnalisisDiagnosticosPlanHCInicial a) => new()
        {
            Id = a.Id,
            Analisis = a.Analisis,
            Plan = a.Plan,
            Diagnosticos = a.Diagnosticos
                .Where(d => d.Cie10Code != null)
                .Select(d => new Cie10CodeResponse
                {
                    Id = d.Cie10Code.Id,
                    Codigo = d.Cie10Code.Codigo,
                    Descripcion = d.Cie10Code.Descripcion,
                    IsActive = d.Cie10Code.IsActive,
                    CreatedAt = d.Cie10Code.CreatedAt,
                    UpdatedAt = d.Cie10Code.UpdatedAt
                }).ToList(),
            IsActive = a.IsActive,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt
        };
    }
}
