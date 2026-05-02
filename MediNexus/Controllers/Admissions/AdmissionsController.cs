using MediNexus.Api.Contracts.Admissions;
using MediNexus.Domain.Admissions;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers.Admissions
{
    [Route("api/admissions")]
    [ApiController]
    [Authorize]
    public class AdmissionsController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public AdmissionsController(MediNexusDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Crea una nueva admisión.
        /// Valida existencia del paciente, triage previo, relación EPS ↔ Convenio,
        /// y que no exista una admisión activa previa.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(AdmissionResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] AdmissionCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 1. Buscar paciente por documento
            var patient = await _db.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.DocumentNumber == request.Document.Trim() && p.IsActive);

            if (patient is null)
                return NotFound(new { message = "Paciente no encontrado o inactivo." });

            // 2. Obtener el último triage activo del paciente
            var latestTriage = await _db.Triages
                .AsNoTracking()
                .Where(t => t.PatientId == patient.Id && t.IsActive)
                .OrderByDescending(t => t.TriageDateTime)
                .FirstOrDefaultAsync();

            if (latestTriage is null)
                return BadRequest(new { message = "El paciente no tiene triage registrado. No se puede crear la admisión." });

            // 3. Validar que el paciente no tenga una admisión activa
            var existsActiveAdmission = await _db.Admissions
                .AnyAsync(a => a.PatientId == patient.Id && a.IsActive);

            if (existsActiveAdmission)
                return Conflict(new { message = "Este paciente ya cuenta con admisión activa." });

            // 4. Validar que la EPS (Insurer) exista y esté activa
            var insurer = await _db.Insurers
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == request.EpsId && i.IsActive);

            if (insurer is null)
                return NotFound(new { message = "La EPS especificada no existe o está inactiva." });

            // 5. Validar que el Convenio (Contract) exista y esté activo
            var contract = await _db.Contracts
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.ConvenioId && c.IsActive);

            if (contract is null)
                return NotFound(new { message = "El convenio especificado no existe o está inactivo." });

            // 6. Validar relación EPS ↔ Convenio: el contrato debe pertenecer a la EPS
            if (contract.InsurerId != request.EpsId)
                return BadRequest(new { message = "El convenio no pertenece a la EPS especificada. La aseguradora debe estar asociada al contrato." });

            // 7. Crear la admisión
            var admission = new Admission
            {
                PatientId = patient.Id,
                TriageId = latestTriage.Id,
                InsurerId = request.EpsId,
                ConvenioId = request.ConvenioId,
                ModalidadAtencion = request.ModalidadAtencion,
                MotivoAtencion = request.MotivoAtencion,
                ClasificacionServicio = request.ClasificacionServicio,
                GrupoServicio = request.GrupoServicio,
                Ingreso = request.Ingreso,
                AmbitoAtencion = request.AmbitoAtencion,
                FinalidadAtencion = request.FinalidadAtencion,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Admissions.Add(admission);
            await _db.SaveChangesAsync();

            var response = new AdmissionResponse
            {
                Id = admission.Id,
                PatientId = patient.Id,
                DocumentoPatiente = patient.DocumentNumber,
                NombrePaciente = $"{patient.FirstName} {patient.LastName}",
                TriageId = latestTriage.Id,
                TriagePrioridad = latestTriage.Priority,
                TriageFechaHora = latestTriage.TriageDateTime,
                ModalidadAtencion = admission.ModalidadAtencion,
                MotivoAtencion = admission.MotivoAtencion,
                ClasificacionServicio = admission.ClasificacionServicio,
                GrupoServicio = admission.GrupoServicio,
                Ingreso = admission.Ingreso,
                AmbitoAtencion = admission.AmbitoAtencion,
                FinalidadAtencion = admission.FinalidadAtencion,
                EpsId = insurer.Id,
                EpsNombre = insurer.Name,
                ConvenioId = contract.Id,
                ConvenioNombre = contract.ContractName,
                IsActive = admission.IsActive,
                CreatedAt = admission.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = admission.Id }, response);
        }

        /// <summary>
        /// Obtiene una admisión por su ID.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(AdmissionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var admission = await _db.Admissions
                .AsNoTracking()
                .Where(a => a.Id == id && a.IsActive)
                .Include(a => a.Patient)
                .Include(a => a.Triage)
                .Include(a => a.Insurer)
                .Include(a => a.Convenio)
                .FirstOrDefaultAsync();

            if (admission is null)
                return NotFound(new { message = "Admisión no encontrada." });

            return Ok(new AdmissionResponse
            {
                Id = admission.Id,
                PatientId = admission.PatientId,
                DocumentoPatiente = admission.Patient.DocumentNumber,
                NombrePaciente = $"{admission.Patient.FirstName} {admission.Patient.LastName}",
                TriageId = admission.TriageId,
                TriagePrioridad = admission.Triage.Priority,
                TriageFechaHora = admission.Triage.TriageDateTime,
                ModalidadAtencion = admission.ModalidadAtencion,
                MotivoAtencion = admission.MotivoAtencion,
                ClasificacionServicio = admission.ClasificacionServicio,
                GrupoServicio = admission.GrupoServicio,
                Ingreso = admission.Ingreso,
                AmbitoAtencion = admission.AmbitoAtencion,
                FinalidadAtencion = admission.FinalidadAtencion,
                EpsId = admission.InsurerId,
                EpsNombre = admission.Insurer.Name,
                ConvenioId = admission.ConvenioId,
                ConvenioNombre = admission.Convenio.ContractName,
                IsActive = admission.IsActive,
                CreatedAt = admission.CreatedAt
            });
        }

        /// <summary>
        /// Lista todas las admisiones activas.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AdmissionResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var admissions = await _db.Admissions
                .AsNoTracking()
                .Where(a => a.IsActive)
                .Include(a => a.Patient)
                .Include(a => a.Triage)
                .Include(a => a.Insurer)
                .Include(a => a.Convenio)
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new AdmissionResponse
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    DocumentoPatiente = a.Patient.DocumentNumber,
                    NombrePaciente = $"{a.Patient.FirstName} {a.Patient.LastName}",
                    TriageId = a.TriageId,
                    TriagePrioridad = a.Triage.Priority,
                    TriageFechaHora = a.Triage.TriageDateTime,
                    ModalidadAtencion = a.ModalidadAtencion,
                    MotivoAtencion = a.MotivoAtencion,
                    ClasificacionServicio = a.ClasificacionServicio,
                    GrupoServicio = a.GrupoServicio,
                    Ingreso = a.Ingreso,
                    AmbitoAtencion = a.AmbitoAtencion,
                    FinalidadAtencion = a.FinalidadAtencion,
                    EpsId = a.InsurerId,
                    EpsNombre = a.Insurer.Name,
                    ConvenioId = a.ConvenioId,
                    ConvenioNombre = a.Convenio.ContractName,
                    IsActive = a.IsActive,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();

            return Ok(admissions);
        }

        /// <summary>
        /// Actualiza una admisión existente.
        /// Revalida la relación EPS ↔ Convenio.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(AdmissionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] AdmissionUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var admission = await _db.Admissions
                .Include(a => a.Patient)
                .Include(a => a.Triage)
                .FirstOrDefaultAsync(a => a.Id == id && a.IsActive);

            if (admission is null)
                return NotFound(new { message = "Admisión no encontrada." });

            // Validar EPS
            var insurer = await _db.Insurers
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == request.EpsId && i.IsActive);

            if (insurer is null)
                return NotFound(new { message = "La EPS especificada no existe o está inactiva." });

            // Validar Convenio
            var contract = await _db.Contracts
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.ConvenioId && c.IsActive);

            if (contract is null)
                return NotFound(new { message = "El convenio especificado no existe o está inactivo." });

            // Validar relación EPS ↔ Convenio
            if (contract.InsurerId != request.EpsId)
                return BadRequest(new { message = "El convenio no pertenece a la EPS especificada. La aseguradora debe estar asociada al contrato." });

            // Actualizar campos
            admission.ModalidadAtencion = request.ModalidadAtencion;
            admission.MotivoAtencion = request.MotivoAtencion;
            admission.ClasificacionServicio = request.ClasificacionServicio;
            admission.GrupoServicio = request.GrupoServicio;
            admission.Ingreso = request.Ingreso;
            admission.AmbitoAtencion = request.AmbitoAtencion;
            admission.FinalidadAtencion = request.FinalidadAtencion;
            admission.InsurerId = request.EpsId;
            admission.ConvenioId = request.ConvenioId;
            admission.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(new AdmissionResponse
            {
                Id = admission.Id,
                PatientId = admission.PatientId,
                DocumentoPatiente = admission.Patient.DocumentNumber,
                NombrePaciente = $"{admission.Patient.FirstName} {admission.Patient.LastName}",
                TriageId = admission.TriageId,
                TriagePrioridad = admission.Triage.Priority,
                TriageFechaHora = admission.Triage.TriageDateTime,
                ModalidadAtencion = admission.ModalidadAtencion,
                MotivoAtencion = admission.MotivoAtencion,
                ClasificacionServicio = admission.ClasificacionServicio,
                GrupoServicio = admission.GrupoServicio,
                Ingreso = admission.Ingreso,
                AmbitoAtencion = admission.AmbitoAtencion,
                FinalidadAtencion = admission.FinalidadAtencion,
                EpsId = admission.InsurerId,
                EpsNombre = insurer.Name,
                ConvenioId = admission.ConvenioId,
                ConvenioNombre = contract.ContractName,
                IsActive = admission.IsActive,
                CreatedAt = admission.CreatedAt
            });
        }

        /// <summary>
        /// Elimina (soft delete) una admisión.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var admission = await _db.Admissions
                .FirstOrDefaultAsync(a => a.Id == id && a.IsActive);

            if (admission is null)
                return NotFound(new { message = "Admisión no encontrada." });

            admission.IsActive = false;
            admission.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
