using MediNexus.Api.Contracts.Triage;
using MediNexus.Domain.Triages;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediNexus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TriageController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public TriageController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/triage
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TriageResponse>>> GetAll()
        {
            var triages = await _db.Triages
                .AsNoTracking()
                .Where(t => t.IsActive)
                .Include(t => t.Patient)
                .OrderByDescending(t => t.TriageDateTime)
                .Select(t => new TriageResponse
                {
                    Id = t.Id,
                    PacienteId = t.PatientId,
                    NumeroDocumento = t.Patient.DocumentNumber,
                    NombrePaciente = $"{t.Patient.FirstName} {t.Patient.LastName}",
                    FechaHora = t.TriageDateTime,
                    Prioridad = t.Priority,
                    MotivoConsulta = t.ConsultationReason,
                    SignosVitales = new VitalSigns
                    {
                        TensionArterial = t.BloodPressure,
                        FrecuenciaCardiaca = t.HeartRate,
                        FrecuenciaRespiratoria = t.RespiratoryRate,
                        Peso = t.Weight,
                        Talla = t.Height,
                        Temperatura = t.Temperature,
                        Glasgow = t.Glasgow
                    },
                    IsActive = t.IsActive
                })
                .ToListAsync();

            return Ok(triages);
        }

        // GET: api/triage/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TriageResponse>> GetById(int id)
        {
            var triage = await _db.Triages
                .AsNoTracking()
                .Where(t => t.Id == id && t.IsActive)
                .Include(t => t.Patient)
                .Select(t => new TriageResponse
                {
                    Id = t.Id,
                    PacienteId = t.PatientId,
                    NumeroDocumento = t.Patient.DocumentNumber,
                    NombrePaciente = $"{t.Patient.FirstName} {t.Patient.LastName}",
                    FechaHora = t.TriageDateTime,
                    Prioridad = t.Priority,
                    MotivoConsulta = t.ConsultationReason,
                    SignosVitales = new VitalSigns
                    {
                        TensionArterial = t.BloodPressure,
                        FrecuenciaCardiaca = t.HeartRate,
                        FrecuenciaRespiratoria = t.RespiratoryRate,
                        Peso = t.Weight,
                        Talla = t.Height,
                        Temperatura = t.Temperature,
                        Glasgow = t.Glasgow
                    },
                    IsActive = t.IsActive
                })
                .FirstOrDefaultAsync();

            if (triage is null)
                return NotFound("Triage not found.");

            return Ok(triage);
        }

        // POST: api/triage
        [HttpPost]
        public async Task<ActionResult<TriageResponse>> RegisterTriage([FromBody] TriageCreateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var patient = await _db.Patients
                .FirstOrDefaultAsync(p => p.DocumentNumber == req.NumeroDocumento.Trim() && p.IsActive);

            if (patient is null)
                return NotFound("Patient not found or inactive.");

            var triage = new Triage
            {
                PatientId = patient.Id,
                TriageDateTime = req.FechaHora,
                Priority = req.Prioridad,
                ConsultationReason = req.MotivoConsulta,
                BloodPressure = req.SignosVitales?.TensionArterial,
                HeartRate = req.SignosVitales?.FrecuenciaCardiaca,
                RespiratoryRate = req.SignosVitales?.FrecuenciaRespiratoria,
                Weight = req.SignosVitales?.Peso,
                Height = req.SignosVitales?.Talla,
                Temperature = req.SignosVitales?.Temperatura,
                Glasgow = req.SignosVitales?.Glasgow,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Triages.Add(triage);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = triage.Id }, new TriageResponse
            {
                Id = triage.Id,
                PacienteId = patient.Id,
                NumeroDocumento = patient.DocumentNumber,
                NombrePaciente = $"{patient.FirstName} {patient.LastName}",
                FechaHora = triage.TriageDateTime,
                Prioridad = triage.Priority,
                MotivoConsulta = triage.ConsultationReason,
                SignosVitales = req.SignosVitales,
                IsActive = triage.IsActive
            });
        }

        // PUT: api/triage/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<TriageResponse>> Update(int id, [FromBody] TriageUpdateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var triage = await _db.Triages
                .Include(t => t.Patient)
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive);

            if (triage is null)
                return NotFound("Triage not found.");

            triage.TriageDateTime = req.FechaHora;
            triage.Priority = req.Prioridad;
            triage.ConsultationReason = req.MotivoConsulta;
            triage.BloodPressure = req.SignosVitales?.TensionArterial;
            triage.HeartRate = req.SignosVitales?.FrecuenciaCardiaca;
            triage.RespiratoryRate = req.SignosVitales?.FrecuenciaRespiratoria;
            triage.Weight = req.SignosVitales?.Peso;
            triage.Height = req.SignosVitales?.Talla;
            triage.Temperature = req.SignosVitales?.Temperatura;
            triage.Glasgow = req.SignosVitales?.Glasgow;
            triage.IsActive = req.IsActive;
            triage.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(new TriageResponse
            {
                Id = triage.Id,
                PacienteId = triage.PatientId,
                NumeroDocumento = triage.Patient.DocumentNumber,
                NombrePaciente = $"{triage.Patient.FirstName} {triage.Patient.LastName}",
                FechaHora = triage.TriageDateTime,
                Prioridad = triage.Priority,
                MotivoConsulta = triage.ConsultationReason,
                SignosVitales = req.SignosVitales,
                IsActive = triage.IsActive
            });
        }

        // DELETE: api/triage/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var triage = await _db.Triages.FirstOrDefaultAsync(t => t.Id == id && t.IsActive);
            if (triage is null)
                return NotFound("Triage not found.");

            triage.IsActive = false;
            triage.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
