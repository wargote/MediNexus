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
    [Route("api/evolucion-especialistas")]
    [ApiController]
    [Authorize]
    public class EvolucionEspecialistaController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public EvolucionEspecialistaController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/evolucion-especialistas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EvolucionEspecialistaResponse>>> GetAll()
        {
            var registros = await _db.EvolucionesEspecialistas
                .AsNoTracking()
                .Where(e => e.IsActive)
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => MapResponse(e))
                .ToListAsync();

            return Ok(registros);
        }

        // GET: api/evolucion-especialistas/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EvolucionEspecialistaResponse>> GetById(int id)
        {
            var registro = await _db.EvolucionesEspecialistas
                .AsNoTracking()
                .Where(e => e.Id == id && e.IsActive)
                .Select(e => MapResponse(e))
                .FirstOrDefaultAsync();

            if (registro is null)
                return NotFound("Evolución de especialista no encontrada.");

            return Ok(registro);
        }

        // GET: api/evolucion-especialistas/by-admission/5
        [HttpGet("by-admission/{admissionId:int}")]
        public async Task<ActionResult<IEnumerable<EvolucionEspecialistaResponse>>> GetByAdmission(int admissionId)
        {
            var registros = await _db.EvolucionesEspecialistas
                .AsNoTracking()
                .Where(e => e.AdmissionId == admissionId && e.IsActive)
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => MapResponse(e))
                .ToListAsync();

            return Ok(registros);
        }

        // POST: api/evolucion-especialistas
        [HttpPost]
        public async Task<ActionResult<EvolucionEspecialistaResponse>> Create([FromBody] EvolucionEspecialistaCreateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var admisionExiste = await _db.Admissions
                .AnyAsync(a => a.Id == req.AdmissionId && a.IsActive);

            if (!admisionExiste)
                return NotFound("Admisión no encontrada o inactiva.");

            var registro = new EvolucionEspecialista
            {
                AdmissionId = req.AdmissionId,
                MotivoConsulta = req.MotivoConsulta.Trim(),
                Plan = req.Plan.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.EvolucionesEspecialistas.Add(registro);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = registro.Id }, MapResponse(registro));
        }

        // PUT: api/evolucion-especialistas/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<EvolucionEspecialistaResponse>> Update(int id, [FromBody] EvolucionEspecialistaUpdateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registro = await _db.EvolucionesEspecialistas
                .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);

            if (registro is null)
                return NotFound("Evolución de especialista no encontrada.");

            registro.MotivoConsulta = req.MotivoConsulta.Trim();
            registro.Plan = req.Plan.Trim();
            registro.IsActive = req.IsActive;
            registro.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(MapResponse(registro));
        }

        // DELETE: api/evolucion-especialistas/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var registro = await _db.EvolucionesEspecialistas
                .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);

            if (registro is null)
                return NotFound("Evolución de especialista no encontrada.");

            registro.IsActive = false;
            registro.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        private static EvolucionEspecialistaResponse MapResponse(EvolucionEspecialista e) => new()
        {
            Id = e.Id,
            AdmissionId = e.AdmissionId,
            MotivoConsulta = e.MotivoConsulta,
            Plan = e.Plan,
            IsActive = e.IsActive,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt
        };
    }
}
