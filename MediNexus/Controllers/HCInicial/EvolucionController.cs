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
    [Route("api/evoluciones")]
    [ApiController]
    [Authorize]
    public class EvolucionController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public EvolucionController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/evoluciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EvolucionResponse>>> GetAll()
        {
            var registros = await _db.Evoluciones
                .AsNoTracking()
                .Where(e => e.IsActive)
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => MapResponse(e))
                .ToListAsync();

            return Ok(registros);
        }

        // GET: api/evoluciones/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EvolucionResponse>> GetById(int id)
        {
            var registro = await _db.Evoluciones
                .AsNoTracking()
                .Where(e => e.Id == id && e.IsActive)
                .Select(e => MapResponse(e))
                .FirstOrDefaultAsync();

            if (registro is null)
                return NotFound("Evolución no encontrada.");

            return Ok(registro);
        }

        // GET: api/evoluciones/by-admission/5
        [HttpGet("by-admission/{admissionId:int}")]
        public async Task<ActionResult<IEnumerable<EvolucionResponse>>> GetByAdmission(int admissionId)
        {
            var registros = await _db.Evoluciones
                .AsNoTracking()
                .Where(e => e.AdmissionId == admissionId && e.IsActive)
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => MapResponse(e))
                .ToListAsync();

            return Ok(registros);
        }

        // POST: api/evoluciones
        [HttpPost]
        public async Task<ActionResult<EvolucionResponse>> Create([FromBody] EvolucionCreateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var admisionExiste = await _db.Admissions
                .AnyAsync(a => a.Id == req.AdmissionId && a.IsActive);

            if (!admisionExiste)
                return NotFound("Admisión no encontrada o inactiva.");

            var registro = new Evolucion
            {
                AdmissionId = req.AdmissionId,
                MotivoConsulta = req.MotivoConsulta.Trim(),
                TensionArterial = req.TensionArterial?.Trim(),
                FrecuenciaCardiaca = req.FrecuenciaCardiaca,
                FrecuenciaRespiratoria = req.FrecuenciaRespiratoria,
                Temperatura = req.Temperatura,
                SaturacionOxigeno = req.SaturacionOxigeno,
                Glasgow = req.Glasgow,
                Peso = req.Peso,
                Talla = req.Talla,
                IMC = CalcularIMC(req.Peso, req.Talla),
                Plan = req.Plan.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Evoluciones.Add(registro);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = registro.Id }, MapResponse(registro));
        }

        // PUT: api/evoluciones/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<EvolucionResponse>> Update(int id, [FromBody] EvolucionUpdateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registro = await _db.Evoluciones
                .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);

            if (registro is null)
                return NotFound("Evolución no encontrada.");

            registro.MotivoConsulta = req.MotivoConsulta.Trim();
            registro.TensionArterial = req.TensionArterial?.Trim();
            registro.FrecuenciaCardiaca = req.FrecuenciaCardiaca;
            registro.FrecuenciaRespiratoria = req.FrecuenciaRespiratoria;
            registro.Temperatura = req.Temperatura;
            registro.SaturacionOxigeno = req.SaturacionOxigeno;
            registro.Glasgow = req.Glasgow;
            registro.Peso = req.Peso;
            registro.Talla = req.Talla;
            registro.IMC = CalcularIMC(req.Peso, req.Talla);
            registro.Plan = req.Plan.Trim();
            registro.IsActive = req.IsActive;
            registro.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(MapResponse(registro));
        }

        // DELETE: api/evoluciones/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var registro = await _db.Evoluciones
                .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);

            if (registro is null)
                return NotFound("Evolución no encontrada.");

            registro.IsActive = false;
            registro.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        private static EvolucionResponse MapResponse(Evolucion e) => new()
        {
            Id = e.Id,
            AdmissionId = e.AdmissionId,
            MotivoConsulta = e.MotivoConsulta,
            TensionArterial = e.TensionArterial,
            FrecuenciaCardiaca = e.FrecuenciaCardiaca,
            FrecuenciaRespiratoria = e.FrecuenciaRespiratoria,
            Temperatura = e.Temperatura,
            SaturacionOxigeno = e.SaturacionOxigeno,
            Glasgow = e.Glasgow,
            Peso = e.Peso,
            Talla = e.Talla,
            IMC = e.IMC,
            Plan = e.Plan,
            IsActive = e.IsActive,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt
        };

        private static decimal? CalcularIMC(decimal? peso, decimal? talla)
        {
            if (peso is null || talla is null || talla == 0)
                return null;

            return Math.Round(peso.Value / (talla.Value * talla.Value), 2);
        }
    }
}
