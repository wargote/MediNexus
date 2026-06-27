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
    [Route("api/signos-vitales-hcinicial")]
    [ApiController]
    [Authorize]
    public class SignosVitalesHCInicialController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public SignosVitalesHCInicialController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/signos-vitales-hcinicial
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SignosVitalesHCInicialResponse>>> GetAll()
        {
            var registros = await _db.SignosVitalesHCInicial
                .AsNoTracking()
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => MapToResponse(s))
                .ToListAsync();

            return Ok(registros);
        }

        // GET: api/signos-vitales-hcinicial/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<SignosVitalesHCInicialResponse>> GetById(int id)
        {
            var registro = await _db.SignosVitalesHCInicial
                .AsNoTracking()
                .Where(s => s.Id == id && s.IsActive)
                .Select(s => MapToResponse(s))
                .FirstOrDefaultAsync();

            if (registro is null)
                return NotFound("Registro de signos vitales de HC Inicial no encontrado.");

            return Ok(registro);
        }

        // POST: api/signos-vitales-hcinicial
        [HttpPost]
        public async Task<ActionResult<SignosVitalesHCInicialResponse>> Create([FromBody] SignosVitalesHCInicialCreateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registro = new SignosVitalesHCInicial
            {
                TensionArterial = req.TensionArterial?.Trim(),
                FrecuenciaCardiaca = req.FrecuenciaCardiaca,
                FrecuenciaRespiratoria = req.FrecuenciaRespiratoria,
                Temperatura = req.Temperatura,
                SaturacionOxigeno = req.SaturacionOxigeno,
                Glasgow = req.Glasgow,
                Peso = req.Peso,
                Talla = req.Talla,
                IMC = CalcularIMC(req.Peso, req.Talla),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.SignosVitalesHCInicial.Add(registro);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = registro.Id }, MapToResponse(registro));
        }

        // PUT: api/signos-vitales-hcinicial/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<SignosVitalesHCInicialResponse>> Update(int id, [FromBody] SignosVitalesHCInicialUpdateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registro = await _db.SignosVitalesHCInicial
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);

            if (registro is null)
                return NotFound("Registro de signos vitales de HC Inicial no encontrado.");

            registro.TensionArterial = req.TensionArterial?.Trim();
            registro.FrecuenciaCardiaca = req.FrecuenciaCardiaca;
            registro.FrecuenciaRespiratoria = req.FrecuenciaRespiratoria;
            registro.Temperatura = req.Temperatura;
            registro.SaturacionOxigeno = req.SaturacionOxigeno;
            registro.Glasgow = req.Glasgow;
            registro.Peso = req.Peso;
            registro.Talla = req.Talla;
            registro.IMC = CalcularIMC(req.Peso, req.Talla);
            registro.IsActive = req.IsActive;
            registro.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(MapToResponse(registro));
        }

        // DELETE: api/signos-vitales-hcinicial/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var registro = await _db.SignosVitalesHCInicial
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);

            if (registro is null)
                return NotFound("Registro de signos vitales de HC Inicial no encontrado.");

            registro.IsActive = false;
            registro.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        private static decimal? CalcularIMC(decimal? peso, decimal? talla)
        {
            if (peso is null || talla is null || talla == 0)
                return null;

            return Math.Round(peso.Value / (talla.Value * talla.Value), 2);
        }

        private static SignosVitalesHCInicialResponse MapToResponse(SignosVitalesHCInicial s) => new()
        {
            Id = s.Id,
            TensionArterial = s.TensionArterial,
            FrecuenciaCardiaca = s.FrecuenciaCardiaca,
            FrecuenciaRespiratoria = s.FrecuenciaRespiratoria,
            Temperatura = s.Temperatura,
            SaturacionOxigeno = s.SaturacionOxigeno,
            Glasgow = s.Glasgow,
            Peso = s.Peso,
            Talla = s.Talla,
            IMC = s.IMC,
            IsActive = s.IsActive,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        };
    }
}
