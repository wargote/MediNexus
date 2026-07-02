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
    [Route("api/objetivo-hcinicial")]
    [ApiController]
    [Authorize]
    public class ObjetivoHCInicialController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public ObjetivoHCInicialController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/objetivo-hcinicial
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObjetivoHCInicialResponse>>> GetAll()
        {
            var registros = await _db.ObjetivosHCInicial
                .AsNoTracking()
                .Where(o => o.IsActive)
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => MapToResponse(o))
                .ToListAsync();

            return Ok(registros);
        }

        // GET: api/objetivo-hcinicial/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ObjetivoHCInicialResponse>> GetById(int id)
        {
            var registro = await _db.ObjetivosHCInicial
                .AsNoTracking()
                .Where(o => o.Id == id && o.IsActive)
                .Select(o => MapToResponse(o))
                .FirstOrDefaultAsync();

            if (registro is null)
                return NotFound("Registro objetivo de HC Inicial no encontrado.");

            return Ok(registro);
        }

        // POST: api/objetivo-hcinicial
        [HttpPost]
        public async Task<ActionResult<ObjetivoHCInicialResponse>> Create([FromBody] ObjetivoHCInicialCreateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registro = new ObjetivoHCInicial
            {
                CabezaCuello = req.CabezaCuello?.Trim(),
                Torax = req.Torax?.Trim(),
                Abdomen = req.Abdomen?.Trim(),
                Extremidades = req.Extremidades?.Trim(),
                SistemaNervioso = req.SistemaNervioso?.Trim(),
                OrganosSentidos = req.OrganosSentidos?.Trim(),
                Genitourinario = req.Genitourinario?.Trim(),
                Padres = req.Padres?.Trim(),
                PersonalesMedicos = req.PersonalesMedicos?.Trim(),
                OtrosFamiliares = req.OtrosFamiliares?.Trim(),
                Alergicos = req.Alergicos?.Trim(),
                Quirurgicos = req.Quirurgicos?.Trim(),
                Toxicos = req.Toxicos?.Trim(),
                Transfusiones = req.Transfusiones?.Trim(),
                Habitos = req.Habitos?.Trim(),
                Traumas = req.Traumas?.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.ObjetivosHCInicial.Add(registro);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = registro.Id }, MapToResponse(registro));
        }

        // PUT: api/objetivo-hcinicial/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ObjetivoHCInicialResponse>> Update(int id, [FromBody] ObjetivoHCInicialUpdateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registro = await _db.ObjetivosHCInicial
                .FirstOrDefaultAsync(o => o.Id == id && o.IsActive);

            if (registro is null)
                return NotFound("Registro objetivo de HC Inicial no encontrado.");

            registro.CabezaCuello = req.CabezaCuello?.Trim();
            registro.Torax = req.Torax?.Trim();
            registro.Abdomen = req.Abdomen?.Trim();
            registro.Extremidades = req.Extremidades?.Trim();
            registro.SistemaNervioso = req.SistemaNervioso?.Trim();
            registro.OrganosSentidos = req.OrganosSentidos?.Trim();
            registro.Genitourinario = req.Genitourinario?.Trim();
            registro.Padres = req.Padres?.Trim();
            registro.PersonalesMedicos = req.PersonalesMedicos?.Trim();
            registro.OtrosFamiliares = req.OtrosFamiliares?.Trim();
            registro.Alergicos = req.Alergicos?.Trim();
            registro.Quirurgicos = req.Quirurgicos?.Trim();
            registro.Toxicos = req.Toxicos?.Trim();
            registro.Transfusiones = req.Transfusiones?.Trim();
            registro.Habitos = req.Habitos?.Trim();
            registro.Traumas = req.Traumas?.Trim();
            registro.IsActive = req.IsActive;
            registro.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(MapToResponse(registro));
        }

        // DELETE: api/objetivo-hcinicial/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var registro = await _db.ObjetivosHCInicial
                .FirstOrDefaultAsync(o => o.Id == id && o.IsActive);

            if (registro is null)
                return NotFound("Registro objetivo de HC Inicial no encontrado.");

            registro.IsActive = false;
            registro.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        private static ObjetivoHCInicialResponse MapToResponse(ObjetivoHCInicial o) => new()
        {
            Id = o.Id,
            CabezaCuello = o.CabezaCuello,
            Torax = o.Torax,
            Abdomen = o.Abdomen,
            Extremidades = o.Extremidades,
            SistemaNervioso = o.SistemaNervioso,
            OrganosSentidos = o.OrganosSentidos,
            Genitourinario = o.Genitourinario,
            Padres = o.Padres,
            PersonalesMedicos = o.PersonalesMedicos,
            OtrosFamiliares = o.OtrosFamiliares,
            Alergicos = o.Alergicos,
            Quirurgicos = o.Quirurgicos,
            Toxicos = o.Toxicos,
            Transfusiones = o.Transfusiones,
            Habitos = o.Habitos,
            Traumas = o.Traumas,
            IsActive = o.IsActive,
            CreatedAt = o.CreatedAt,
            UpdatedAt = o.UpdatedAt
        };
    }
}
