using MediNexus.Api.Contracts.Patient;
using MediNexus.Domain.Patients;
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
    public class PatientsController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public PatientsController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/patients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientResponse>>> GetAll([FromQuery] string? search)
        {
            var query = _db.Patients
                .AsNoTracking()
                .Where(p => p.IsActive)
                .Include(p => p.Insurer)
                .Include(p => p.DocumentType)
                .Include(p => p.BirthCountry)
                .Include(p => p.ResidenceCountry)
                .Include(p => p.State)
                .Include(p => p.City)
                .Include(p => p.Sex)
                .Include(p => p.Disability)
                .Include(p => p.Zone)
                .Include(p => p.BloodGroup)
                .Include(p => p.RhFactor)
                .Include(p => p.MaritalStatus)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = $"%{search.Trim()}%";
                query = query.Where(p =>
                    EF.Functions.ILike(p.DocumentNumber, term) ||
                    EF.Functions.ILike(p.FirstName, term) ||
                    EF.Functions.ILike(p.MiddleName!, term) ||
                    EF.Functions.ILike(p.LastName, term) ||
                    EF.Functions.ILike(p.SecondLastName!, term));
            }

            var patients = await query
                .OrderBy(p => p.Id)
                .Select(p => new PatientResponse
                {
                    Id = p.Id,
                    InsurerId = p.InsurerId,
                    InsurerName = p.Insurer.Name,
                    DocumentTypeId = p.DocumentTypeId,
                    DocumentTypeCode = p.DocumentType.Code,
                    DocumentTypeName = p.DocumentType.Name,
                    DocumentNumber = p.DocumentNumber,
                    FirstName = p.FirstName,
                    MiddleName = p.MiddleName,
                    LastName = p.LastName,
                    SecondLastName = p.SecondLastName,
                    BirthDate = p.BirthDate,
                    SexId = p.SexId,
                    SexName = p.Sex != null ? p.Sex.Name : null,
                    BirthCountryId = p.BirthCountryId,
                    BirthCountryName = p.BirthCountry.Name,
                    ResidenceCountryId = p.ResidenceCountryId,
                    ResidenceCountryName = p.ResidenceCountry.Name,
                    StateId = p.StateId,
                    StateName = p.State.Name,
                    CityId = p.CityId,
                    CityName = p.City.Name,
                    ZoneId = p.ZoneId,
                    ZoneName = p.Zone != null ? p.Zone.Name : null,
                    Address = p.Address,
                    Phone = p.Phone,
                    Email = p.Email,
                    MaritalStatusId = p.MaritalStatusId,
                    MaritalStatusName = p.MaritalStatus != null ? p.MaritalStatus.Name : null,
                    DisabilityId = p.DisabilityId,
                    DisabilityName = p.Disability != null ? p.Disability.Name : null,
                    BloodGroupId = p.BloodGroupId,
                    BloodGroupName = p.BloodGroup != null ? p.BloodGroup.Name : null,
                    RhFactorId = p.RhFactorId,
                    RhFactorName = p.RhFactor != null ? p.RhFactor.Name : null,
                    IsActive = p.IsActive
                })
                .ToListAsync();

            return Ok(patients);
        }

        // GET: api/patients/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PatientResponse>> GetById(int id)
        {
            var patient = await _db.Patients
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Include(p => p.Insurer)
                .Include(p => p.DocumentType)
                .Include(p => p.BirthCountry)
                .Include(p => p.ResidenceCountry)
                .Include(p => p.State)
                .Include(p => p.City)
                .Include(p => p.Sex)
                .Include(p => p.Disability)
                .Include(p => p.Zone)
                .Include(p => p.BloodGroup)
                .Include(p => p.RhFactor)
                .Include(p => p.MaritalStatus)
                .Select(p => new PatientResponse
                {
                    Id = p.Id,
                    InsurerId = p.InsurerId,
                    InsurerName = p.Insurer.Name,
                    DocumentTypeId = p.DocumentTypeId,
                    DocumentTypeCode = p.DocumentType.Code,
                    DocumentTypeName = p.DocumentType.Name,
                    DocumentNumber = p.DocumentNumber,
                    FirstName = p.FirstName,
                    MiddleName = p.MiddleName,
                    LastName = p.LastName,
                    SecondLastName = p.SecondLastName,
                    BirthDate = p.BirthDate,
                    SexId = p.SexId,
                    SexName = p.Sex != null ? p.Sex.Name : null,
                    BirthCountryId = p.BirthCountryId,
                    BirthCountryName = p.BirthCountry.Name,
                    ResidenceCountryId = p.ResidenceCountryId,
                    ResidenceCountryName = p.ResidenceCountry.Name,
                    StateId = p.StateId,
                    StateName = p.State.Name,
                    CityId = p.CityId,
                    CityName = p.City.Name,
                    ZoneId = p.ZoneId,
                    ZoneName = p.Zone != null ? p.Zone.Name : null,
                    Address = p.Address,
                    Phone = p.Phone,
                    Email = p.Email,
                    MaritalStatusId = p.MaritalStatusId,
                    MaritalStatusName = p.MaritalStatus != null ? p.MaritalStatus.Name : null,
                    DisabilityId = p.DisabilityId,
                    DisabilityName = p.Disability != null ? p.Disability.Name : null,
                    BloodGroupId = p.BloodGroupId,
                    BloodGroupName = p.BloodGroup != null ? p.BloodGroup.Name : null,
                    RhFactorId = p.RhFactorId,
                    RhFactorName = p.RhFactor != null ? p.RhFactor.Name : null,
                    IsActive = p.IsActive
                })
                .FirstOrDefaultAsync();

            if (patient is null)
                return NotFound("Patient not found.");

            return Ok(patient);
        }

        [HttpGet("by-document/{documentNumber}")]
        public async Task<ActionResult<PatientMiniResponse>> GetByDocument(string documentNumber)
        {
            var patient = await _db.Patients
                .AsNoTracking()
                .Where(p => p.DocumentNumber == documentNumber.Trim() && p.IsActive)
                .Include(p => p.Sex)
                .Select(p => new PatientMiniResponse
                {
                    PrimerNombre = p.FirstName,
                    SegundoNombre = p.MiddleName,
                    PrimerApellido = p.LastName,
                    SegundoApellido = p.SecondLastName,
                    FechaNacimiento = p.BirthDate,
                    Sexo = p.Sex != null ? p.Sex.Name : null
                })
                .FirstOrDefaultAsync();

            if (patient is null)
                return NotFound("Patient not found.");

            return Ok(patient);
        }

        // POST: api/patients
        [HttpPost]
        public async Task<ActionResult<PatientResponse>> Create([FromBody] PatientCreateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _db.Insurers.AnyAsync(i => i.Id == req.InsurerId && i.IsActive))
                return BadRequest("Invalid insurer.");

            if (!await _db.DocumentTypes.AnyAsync(d => d.Id == req.DocumentTypeId && d.IsActive))
                return BadRequest("Invalid document type.");

            if (!await _db.Countries.AnyAsync(c => c.Id == req.BirthCountryId && c.IsActive))
                return BadRequest("Invalid birth country.");

            if (!await _db.Countries.AnyAsync(c => c.Id == req.ResidenceCountryId && c.IsActive))
                return BadRequest("Invalid residence country.");

            if (!await _db.States.AnyAsync(s => s.Id == req.StateId && s.IsActive))
                return BadRequest("Invalid state.");

            if (!await _db.Cities.AnyAsync(c => c.Id == req.CityId && c.StateId == req.StateId && c.IsActive))
                return BadRequest("Invalid city for the selected state.");

            if (req.SexId.HasValue && !await _db.Sexes.AnyAsync(s => s.Id == req.SexId && s.IsActive))
                return BadRequest("Invalid sex.");

            if (req.ZoneId.HasValue && !await _db.Zones.AnyAsync(z => z.Id == req.ZoneId && z.IsActive))
                return BadRequest("Invalid zone.");

            if (req.MaritalStatusId.HasValue && !await _db.MaritalStatuses.AnyAsync(m => m.Id == req.MaritalStatusId && m.IsActive))
                return BadRequest("Invalid marital status.");

            if (req.DisabilityId.HasValue && !await _db.Disabilities.AnyAsync(d => d.Id == req.DisabilityId && d.IsActive))
                return BadRequest("Invalid disability.");

            if (req.BloodGroupId.HasValue && !await _db.BloodGroups.AnyAsync(b => b.Id == req.BloodGroupId && b.IsActive))
                return BadRequest("Invalid blood group.");

            if (req.RhFactorId.HasValue && !await _db.RhFactors.AnyAsync(r => r.Id == req.RhFactorId && r.IsActive))
                return BadRequest("Invalid Rh factor.");

            var documentNumber = req.DocumentNumber.Trim();

            if (await _db.Patients.AnyAsync(p => p.DocumentTypeId == req.DocumentTypeId && p.DocumentNumber == documentNumber))
                return Conflict("A patient with this document type and number already exists.");

            var patient = new Patient
            {
                InsurerId = req.InsurerId,
                DocumentTypeId = req.DocumentTypeId,
                DocumentNumber = documentNumber,
                FirstName = req.FirstName.Trim(),
                MiddleName = string.IsNullOrWhiteSpace(req.MiddleName) ? null : req.MiddleName.Trim(),
                LastName = req.LastName.Trim(),
                SecondLastName = string.IsNullOrWhiteSpace(req.SecondLastName) ? null : req.SecondLastName.Trim(),
                BirthDate = req.BirthDate,
                SexId = req.SexId,
                BirthCountryId = req.BirthCountryId,
                ResidenceCountryId = req.ResidenceCountryId,
                StateId = req.StateId,
                CityId = req.CityId,
                ZoneId = req.ZoneId,
                Address = string.IsNullOrWhiteSpace(req.Address) ? null : req.Address.Trim(),
                Phone = string.IsNullOrWhiteSpace(req.Phone) ? null : req.Phone.Trim(),
                Email = string.IsNullOrWhiteSpace(req.Email) ? null : req.Email.Trim(),
                MaritalStatusId = req.MaritalStatusId,
                DisabilityId = req.DisabilityId,
                BloodGroupId = req.BloodGroupId,
                RhFactorId = req.RhFactorId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Patients.Add(patient);
            await _db.SaveChangesAsync();

            var createdAction = await GetById(patient.Id);
            var createdResult = createdAction.Result as OkObjectResult;
            return CreatedAtAction(nameof(GetById), new { id = patient.Id }, createdResult?.Value);
        }

        // PUT: api/patients/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<PatientResponse>> Update(int id, [FromBody] PatientUpdateRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var patient = await _db.Patients.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
            if (patient is null)
                return NotFound("Patient not found or inactive.");

            if (!await _db.Insurers.AnyAsync(i => i.Id == req.InsurerId && i.IsActive))
                return BadRequest("Invalid insurer.");

            if (!await _db.DocumentTypes.AnyAsync(d => d.Id == req.DocumentTypeId && d.IsActive))
                return BadRequest("Invalid document type.");

            if (!await _db.Countries.AnyAsync(c => c.Id == req.BirthCountryId && c.IsActive))
                return BadRequest("Invalid birth country.");

            if (!await _db.Countries.AnyAsync(c => c.Id == req.ResidenceCountryId && c.IsActive))
                return BadRequest("Invalid residence country.");

            if (!await _db.States.AnyAsync(s => s.Id == req.StateId && s.IsActive))
                return BadRequest("Invalid state.");

            if (!await _db.Cities.AnyAsync(c => c.Id == req.CityId && c.StateId == req.StateId && c.IsActive))
                return BadRequest("Invalid city for the selected state.");

            if (req.SexId.HasValue && !await _db.Sexes.AnyAsync(s => s.Id == req.SexId && s.IsActive))
                return BadRequest("Invalid sex.");

            if (req.ZoneId.HasValue && !await _db.Zones.AnyAsync(z => z.Id == req.ZoneId && z.IsActive))
                return BadRequest("Invalid zone.");

            if (req.MaritalStatusId.HasValue && !await _db.MaritalStatuses.AnyAsync(m => m.Id == req.MaritalStatusId && m.IsActive))
                return BadRequest("Invalid marital status.");

            if (req.DisabilityId.HasValue && !await _db.Disabilities.AnyAsync(d => d.Id == req.DisabilityId && d.IsActive))
                return BadRequest("Invalid disability.");

            if (req.BloodGroupId.HasValue && !await _db.BloodGroups.AnyAsync(b => b.Id == req.BloodGroupId && b.IsActive))
                return BadRequest("Invalid blood group.");

            if (req.RhFactorId.HasValue && !await _db.RhFactors.AnyAsync(r => r.Id == req.RhFactorId && r.IsActive))
                return BadRequest("Invalid Rh factor.");

            var documentNumber = req.DocumentNumber.Trim();

            if (await _db.Patients.AnyAsync(p => p.Id != id && p.DocumentTypeId == req.DocumentTypeId && p.DocumentNumber == documentNumber))
                return Conflict("A patient with this document type and number already exists.");

            patient.InsurerId = req.InsurerId;
            patient.DocumentTypeId = req.DocumentTypeId;
            patient.DocumentNumber = documentNumber;
            patient.FirstName = req.FirstName.Trim();
            patient.MiddleName = string.IsNullOrWhiteSpace(req.MiddleName) ? null : req.MiddleName.Trim();
            patient.LastName = req.LastName.Trim();
            patient.SecondLastName = string.IsNullOrWhiteSpace(req.SecondLastName) ? null : req.SecondLastName.Trim();
            patient.BirthDate = req.BirthDate;
            patient.SexId = req.SexId;
            patient.BirthCountryId = req.BirthCountryId;
            patient.ResidenceCountryId = req.ResidenceCountryId;
            patient.StateId = req.StateId;
            patient.CityId = req.CityId;
            patient.ZoneId = req.ZoneId;
            patient.Address = string.IsNullOrWhiteSpace(req.Address) ? null : req.Address.Trim();
            patient.Phone = string.IsNullOrWhiteSpace(req.Phone) ? null : req.Phone.Trim();
            patient.Email = string.IsNullOrWhiteSpace(req.Email) ? null : req.Email.Trim();
            patient.MaritalStatusId = req.MaritalStatusId;
            patient.DisabilityId = req.DisabilityId;
            patient.BloodGroupId = req.BloodGroupId;
            patient.RhFactorId = req.RhFactorId;
            patient.IsActive = req.IsActive;
            patient.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            var updatedAction = await GetById(patient.Id);
            var updatedResult = updatedAction.Result as OkObjectResult;
            return Ok(updatedResult?.Value);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _db.Patients.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
            if (patient is null)
                return NotFound("Patient not found.");

            patient.IsActive = false;
            patient.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("catalogs/sexes")]
        public async Task<ActionResult<IEnumerable<object>>> GetSexes()
        {
            return await _db.Sexes.Where(x => x.IsActive).OrderBy(x => x.Name).Select(x => new { x.Id, x.Name }).ToListAsync();
        }

        [HttpGet("catalogs/disabilities")]
        public async Task<ActionResult<IEnumerable<object>>> GetDisabilities()
        {
            return await _db.Disabilities.Where(x => x.IsActive).OrderBy(x => x.Name).Select(x => new { x.Id, x.Name }).ToListAsync();
        }

        [HttpGet("catalogs/zones")]
        public async Task<ActionResult<IEnumerable<object>>> GetZones()
        {
            return await _db.Zones.Where(x => x.IsActive).OrderBy(x => x.Name).Select(x => new { x.Id, x.Name }).ToListAsync();
        }

        [HttpGet("catalogs/blood-groups")]
        public async Task<ActionResult<IEnumerable<object>>> GetBloodGroups()
        {
            return await _db.BloodGroups.Where(x => x.IsActive).OrderBy(x => x.Name).Select(x => new { x.Id, x.Name }).ToListAsync();
        }

        [HttpGet("catalogs/rh-factors")]
        public async Task<ActionResult<IEnumerable<object>>> GetRhFactors()
        {
            return await _db.RhFactors.Where(x => x.IsActive).OrderBy(x => x.Name).Select(x => new { x.Id, x.Name }).ToListAsync();
        }

        [HttpGet("catalogs/marital-statuses")]
        public async Task<ActionResult<IEnumerable<object>>> GetMaritalStatuses()
        {
            return await _db.MaritalStatuses.Where(x => x.IsActive).OrderBy(x => x.Name).Select(x => new { x.Id, x.Name }).ToListAsync();
        }
    }
}
