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
    [Route("api/subjetivo-hcinicial")]
    [ApiController]
    [Authorize]
    public class SubjetivoHCInicialController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public SubjetivoHCInicialController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/subjetivo-hcinicial
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjetivoHCInicialResponse>>> GetAll()
        {
            var registros = await _db.SubjetivosHCInicial
                .AsNoTracking()
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => new SubjetivoHCInicialResponse
                {
                    Id = s.Id,
                    MotivoConsulta = s.MotivoConsulta,
                    EnfermedadActual = s.EnfermedadActual,
                    IsActive = s.IsActive,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                })
                .ToListAsync();

            return Ok(registros);
        }

        // GET: api/subjetivo-hcinicial/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<SubjetivoHCInicialResponse>> GetById(int id)
        {
            var registro = await _db.SubjetivosHCInicial
                .AsNoTracking()
                .Where(s => s.Id == id && s.IsActive)
                .Select(s => new SubjetivoHCInicialResponse
                {
                    Id = s.Id,
                    MotivoConsulta = s.MotivoConsulta,
                    EnfermedadActual = s.EnfermedadActual,
                    IsActive = s.IsActive,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (registro is null)
                return NotFound("Registro subjetivo de HC Inicial no encontrado.");

            return Ok(registro);
        }

        // POST: api/subjetivo-hcinicial
        [HttpPost]
        public async Task<ActionResult<SubjetivoHCInicialResponse>> Create([FromBody] SubjetivoHCInicialCreateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registro = new SubjetivoHCInicial
            {
                MotivoConsulta = req.MotivoConsulta.Trim(),
                EnfermedadActual = req.EnfermedadActual.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.SubjetivosHCInicial.Add(registro);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = registro.Id }, new SubjetivoHCInicialResponse
            {
                Id = registro.Id,
                MotivoConsulta = registro.MotivoConsulta,
                EnfermedadActual = registro.EnfermedadActual,
                IsActive = registro.IsActive,
                CreatedAt = registro.CreatedAt,
                UpdatedAt = registro.UpdatedAt
            });
        }

        // PUT: api/subjetivo-hcinicial/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<SubjetivoHCInicialResponse>> Update(int id, [FromBody] SubjetivoHCInicialUpdateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registro = await _db.SubjetivosHCInicial
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);

            if (registro is null)
                return NotFound("Registro subjetivo de HC Inicial no encontrado.");

            registro.MotivoConsulta = req.MotivoConsulta.Trim();
            registro.EnfermedadActual = req.EnfermedadActual.Trim();
            registro.IsActive = req.IsActive;
            registro.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(new SubjetivoHCInicialResponse
            {
                Id = registro.Id,
                MotivoConsulta = registro.MotivoConsulta,
                EnfermedadActual = registro.EnfermedadActual,
                IsActive = registro.IsActive,
                CreatedAt = registro.CreatedAt,
                UpdatedAt = registro.UpdatedAt
            });
        }

        // DELETE: api/subjetivo-hcinicial/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var registro = await _db.SubjetivosHCInicial
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);

            if (registro is null)
                return NotFound("Registro subjetivo de HC Inicial no encontrado.");

            registro.IsActive = false;
            registro.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
