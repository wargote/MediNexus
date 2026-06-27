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
    [Route("api/hcinicial")]
    [ApiController]
    [Authorize]
    public class HCInicialController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public HCInicialController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/hcinicial
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HCInicialResponse>>> GetAll()
        {
            var registros = await _db.HCIniciales
                .AsNoTracking()
                .Where(h => h.IsActive)
                .Include(h => h.Admission).ThenInclude(a => a.Patient)
                .Include(h => h.Subjetivo)
                .Include(h => h.Objetivo)
                .Include(h => h.SignosVitales)
                .Include(h => h.AnalisisDiagnosticosPlan).ThenInclude(a => a.Diagnosticos).ThenInclude(d => d.Cie10Code)
                .OrderByDescending(h => h.CreatedAt)
                .Select(h => BuildResponse(h))
                .ToListAsync();

            return Ok(registros);
        }

        // GET: api/hcinicial/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<HCInicialResponse>> GetById(int id)
        {
            var h = await _db.HCIniciales
                .AsNoTracking()
                .Where(h => h.Id == id && h.IsActive)
                .Include(h => h.Admission).ThenInclude(a => a.Patient)
                .Include(h => h.Subjetivo)
                .Include(h => h.Objetivo)
                .Include(h => h.SignosVitales)
                .Include(h => h.AnalisisDiagnosticosPlan).ThenInclude(a => a.Diagnosticos).ThenInclude(d => d.Cie10Code)
                .FirstOrDefaultAsync();

            if (h is null)
                return NotFound("HC Inicial no encontrada.");

            return Ok(BuildResponse(h));
        }

        // GET: api/hcinicial/by-admission/5
        [HttpGet("by-admission/{admissionId:int}")]
        public async Task<ActionResult<HCInicialResponse>> GetByAdmission(int admissionId)
        {
            var h = await _db.HCIniciales
                .AsNoTracking()
                .Where(h => h.AdmissionId == admissionId && h.IsActive)
                .Include(h => h.Admission).ThenInclude(a => a.Patient)
                .Include(h => h.Subjetivo)
                .Include(h => h.Objetivo)
                .Include(h => h.SignosVitales)
                .Include(h => h.AnalisisDiagnosticosPlan).ThenInclude(a => a.Diagnosticos).ThenInclude(d => d.Cie10Code)
                .FirstOrDefaultAsync();

            if (h is null)
                return NotFound("No existe HC Inicial activa para esta admisión.");

            return Ok(BuildResponse(h));
        }

        // POST: api/hcinicial
        [HttpPost]
        public async Task<ActionResult<HCInicialResponse>> Create([FromBody] HCInicialCreateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var admission = await _db.Admissions
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == req.AdmissionId && a.IsActive);

            if (admission is null)
                return NotFound("Admisión no encontrada o inactiva.");

            var yaExiste = await _db.HCIniciales
                .AnyAsync(h => h.AdmissionId == req.AdmissionId && h.IsActive);

            if (yaExiste)
                return Conflict(new { message = "Ya existe una HC Inicial activa para esta admisión." });

            if (req.IdSubjetivoHCInicial.HasValue)
            {
                var existe = await _db.SubjetivosHCInicial
                    .AnyAsync(s => s.Id == req.IdSubjetivoHCInicial.Value && s.IsActive);
                if (!existe)
                    return NotFound("El registro subjetivo referenciado no existe o está inactivo.");
            }

            if (req.IdObjetivoHCInicial.HasValue)
            {
                var existe = await _db.ObjetivosHCInicial
                    .AnyAsync(o => o.Id == req.IdObjetivoHCInicial.Value && o.IsActive);
                if (!existe)
                    return NotFound("El registro objetivo referenciado no existe o está inactivo.");
            }

            if (req.IdSignosVitalesHCInicial.HasValue)
            {
                var existe = await _db.SignosVitalesHCInicial
                    .AnyAsync(s => s.Id == req.IdSignosVitalesHCInicial.Value && s.IsActive);
                if (!existe)
                    return NotFound("El registro de signos vitales referenciado no existe o está inactivo.");
            }

            if (req.IdAnalisisDiagnosticosPlanHCInicial.HasValue)
            {
                var existe = await _db.AnalisisDiagnosticosPlanHCInicial
                    .AnyAsync(a => a.Id == req.IdAnalisisDiagnosticosPlanHCInicial.Value && a.IsActive);
                if (!existe)
                    return NotFound("El registro de análisis, diagnósticos y plan referenciado no existe o está inactivo.");
            }

            var hcInicial = new Domain.HCInicial.HCInicial
            {
                AdmissionId = req.AdmissionId,
                IdSubjetivoHCInicial = req.IdSubjetivoHCInicial,
                IdObjetivoHCInicial = req.IdObjetivoHCInicial,
                IdSignosVitalesHCInicial = req.IdSignosVitalesHCInicial,
                IdAnalisisDiagnosticosPlanHCInicial = req.IdAnalisisDiagnosticosPlanHCInicial,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.HCIniciales.Add(hcInicial);
            await _db.SaveChangesAsync();

            await _db.Entry(hcInicial).Reference(h => h.Subjetivo).LoadAsync();
            await _db.Entry(hcInicial).Reference(h => h.Objetivo).LoadAsync();
            await _db.Entry(hcInicial).Reference(h => h.SignosVitales).LoadAsync();
            await _db.Entry(hcInicial).Reference(h => h.AnalisisDiagnosticosPlan).LoadAsync();
            if (hcInicial.AnalisisDiagnosticosPlan != null)
                await _db.Entry(hcInicial.AnalisisDiagnosticosPlan).Collection(a => a.Diagnosticos).Query()
                    .Include(d => d.Cie10Code).LoadAsync();

            return CreatedAtAction(nameof(GetById), new { id = hcInicial.Id }, BuildResponseWithAdmission(hcInicial, admission));
        }

        // PUT: api/hcinicial/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<HCInicialResponse>> Update(int id, [FromBody] HCInicialUpdateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hcInicial = await _db.HCIniciales
                .Include(h => h.Admission).ThenInclude(a => a.Patient)
                .FirstOrDefaultAsync(h => h.Id == id && h.IsActive);

            if (hcInicial is null)
                return NotFound("HC Inicial no encontrada.");

            if (req.IdSubjetivoHCInicial.HasValue)
            {
                var existe = await _db.SubjetivosHCInicial
                    .AnyAsync(s => s.Id == req.IdSubjetivoHCInicial.Value && s.IsActive);
                if (!existe)
                    return NotFound("El registro subjetivo referenciado no existe o está inactivo.");
            }

            if (req.IdObjetivoHCInicial.HasValue)
            {
                var existe = await _db.ObjetivosHCInicial
                    .AnyAsync(o => o.Id == req.IdObjetivoHCInicial.Value && o.IsActive);
                if (!existe)
                    return NotFound("El registro objetivo referenciado no existe o está inactivo.");
            }

            if (req.IdSignosVitalesHCInicial.HasValue)
            {
                var existe = await _db.SignosVitalesHCInicial
                    .AnyAsync(s => s.Id == req.IdSignosVitalesHCInicial.Value && s.IsActive);
                if (!existe)
                    return NotFound("El registro de signos vitales referenciado no existe o está inactivo.");
            }

            if (req.IdAnalisisDiagnosticosPlanHCInicial.HasValue)
            {
                var existe = await _db.AnalisisDiagnosticosPlanHCInicial
                    .AnyAsync(a => a.Id == req.IdAnalisisDiagnosticosPlanHCInicial.Value && a.IsActive);
                if (!existe)
                    return NotFound("El registro de análisis, diagnósticos y plan referenciado no existe o está inactivo.");
            }

            hcInicial.IdSubjetivoHCInicial = req.IdSubjetivoHCInicial;
            hcInicial.IdObjetivoHCInicial = req.IdObjetivoHCInicial;
            hcInicial.IdSignosVitalesHCInicial = req.IdSignosVitalesHCInicial;
            hcInicial.IdAnalisisDiagnosticosPlanHCInicial = req.IdAnalisisDiagnosticosPlanHCInicial;
            hcInicial.IsActive = req.IsActive;
            hcInicial.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            await _db.Entry(hcInicial).Reference(h => h.Subjetivo).LoadAsync();
            await _db.Entry(hcInicial).Reference(h => h.Objetivo).LoadAsync();
            await _db.Entry(hcInicial).Reference(h => h.SignosVitales).LoadAsync();
            await _db.Entry(hcInicial).Reference(h => h.AnalisisDiagnosticosPlan).LoadAsync();
            if (hcInicial.AnalisisDiagnosticosPlan != null)
                await _db.Entry(hcInicial.AnalisisDiagnosticosPlan).Collection(a => a.Diagnosticos).Query()
                    .Include(d => d.Cie10Code).LoadAsync();

            return Ok(BuildResponse(hcInicial));
        }

        // DELETE: api/hcinicial/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var hcInicial = await _db.HCIniciales
                .FirstOrDefaultAsync(h => h.Id == id && h.IsActive);

            if (hcInicial is null)
                return NotFound("HC Inicial no encontrada.");

            hcInicial.IsActive = false;
            hcInicial.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        private static HCInicialResponse BuildResponse(Domain.HCInicial.HCInicial h) => new()
        {
            Id = h.Id,
            AdmissionId = h.AdmissionId,
            NombrePaciente = $"{h.Admission.Patient.FirstName} {h.Admission.Patient.LastName}",
            DocumentoPaciente = h.Admission.Patient.DocumentNumber,
            IdSubjetivoHCInicial = h.IdSubjetivoHCInicial,
            Subjetivo = h.Subjetivo == null ? null : MapSubjetivo(h.Subjetivo),
            IdObjetivoHCInicial = h.IdObjetivoHCInicial,
            Objetivo = h.Objetivo == null ? null : MapObjetivo(h.Objetivo),
            IdSignosVitalesHCInicial = h.IdSignosVitalesHCInicial,
            SignosVitales = h.SignosVitales == null ? null : MapSignosVitales(h.SignosVitales),
            IdAnalisisDiagnosticosPlanHCInicial = h.IdAnalisisDiagnosticosPlanHCInicial,
            AnalisisDiagnosticosPlan = h.AnalisisDiagnosticosPlan == null ? null : MapAnalisisDiagnosticosPlan(h.AnalisisDiagnosticosPlan),
            IsActive = h.IsActive,
            CreatedAt = h.CreatedAt,
            UpdatedAt = h.UpdatedAt
        };

        private static HCInicialResponse BuildResponseWithAdmission(Domain.HCInicial.HCInicial h, Domain.Admissions.Admission admission) => new()
        {
            Id = h.Id,
            AdmissionId = h.AdmissionId,
            NombrePaciente = $"{admission.Patient.FirstName} {admission.Patient.LastName}",
            DocumentoPaciente = admission.Patient.DocumentNumber,
            IdSubjetivoHCInicial = h.IdSubjetivoHCInicial,
            Subjetivo = h.Subjetivo == null ? null : MapSubjetivo(h.Subjetivo),
            IdObjetivoHCInicial = h.IdObjetivoHCInicial,
            Objetivo = h.Objetivo == null ? null : MapObjetivo(h.Objetivo),
            IdSignosVitalesHCInicial = h.IdSignosVitalesHCInicial,
            SignosVitales = h.SignosVitales == null ? null : MapSignosVitales(h.SignosVitales),
            IdAnalisisDiagnosticosPlanHCInicial = h.IdAnalisisDiagnosticosPlanHCInicial,
            AnalisisDiagnosticosPlan = h.AnalisisDiagnosticosPlan == null ? null : MapAnalisisDiagnosticosPlan(h.AnalisisDiagnosticosPlan),
            IsActive = h.IsActive,
            CreatedAt = h.CreatedAt,
            UpdatedAt = h.UpdatedAt
        };

        private static SubjetivoHCInicialResponse MapSubjetivo(SubjetivoHCInicial s) => new()
        {
            Id = s.Id,
            MotivoConsulta = s.MotivoConsulta,
            EnfermedadActual = s.EnfermedadActual,
            IsActive = s.IsActive,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        };

        private static ObjetivoHCInicialResponse MapObjetivo(ObjetivoHCInicial o) => new()
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

        private static SignosVitalesHCInicialResponse MapSignosVitales(SignosVitalesHCInicial s) => new()
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

        private static AnalisisDiagnosticosPlanHCInicialResponse MapAnalisisDiagnosticosPlan(AnalisisDiagnosticosPlanHCInicial a) => new()
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
