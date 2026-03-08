using MediNexus.Api.Contracts.Medicines;
using MediNexus.Domain.Medicines;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers.Medicines
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicinesController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public MedicinesController(MediNexusDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<MedicineResponse>>> GetAll([FromQuery] string? search = null)
        {
            var query = _db.Medicines
                .AsNoTracking()
                .Include(x => x.MeasurementUnit)
                .Include(x => x.AdministrationRoute)
                .Include(x => x.PharmaceuticalForm)
                .Include(x => x.Presentation)
                .Include(x => x.MedicineGroup)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x =>
                    x.Name.Contains(search) ||
                    (x.Cum != null && x.Cum.Contains(search)) ||
                    (x.Atc != null && x.Atc.Contains(search)) ||
                    (x.Invima != null && x.Invima.Contains(search)));
            }

            var items = await query
                .OrderBy(x => x.Name)
                .Select(x => new MedicineResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Cum = x.Cum,
                    Concentration = x.Concentration,
                    MeasurementUnitSidamId = x.MeasurementUnitSidamId,
                    AdministrationRouteCode = x.AdministrationRouteCode,
                    PharmaceuticalFormCode = x.PharmaceuticalFormCode,
                    PresentationCode = x.PresentationCode,
                    MedicineGroupCode = x.MedicineGroupCode,
                    Atc = x.Atc,
                    Invima = x.Invima,
                    Price = x.Price,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,

                    MeasurementUnitDescription = x.MeasurementUnit != null
                        ? $"{x.MeasurementUnit.UnitCode} - {x.MeasurementUnit.Description}"
                        : null,
                    AdministrationRouteDescription = x.AdministrationRoute != null ? x.AdministrationRoute.Description : null,
                    PharmaceuticalFormDescription = x.PharmaceuticalForm != null ? x.PharmaceuticalForm.Description : null,
                    PresentationDescription = x.Presentation != null ? x.Presentation.Description : null,
                    MedicineGroupDescription = x.MedicineGroup != null ? x.MedicineGroup.Description : null
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<MedicineResponse>> GetById(long id)
        {
            var x = await _db.Medicines
                .AsNoTracking()
                .Include(m => m.MeasurementUnit)
                .Include(m => m.AdministrationRoute)
                .Include(m => m.PharmaceuticalForm)
                .Include(m => m.Presentation)
                .Include(m => m.MedicineGroup)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (x == null) return NotFound();

            return Ok(new MedicineResponse
            {
                Id = x.Id,
                Name = x.Name,
                Cum = x.Cum,
                Concentration = x.Concentration,
                MeasurementUnitSidamId = x.MeasurementUnitSidamId,
                AdministrationRouteCode = x.AdministrationRouteCode,
                PharmaceuticalFormCode = x.PharmaceuticalFormCode,
                PresentationCode = x.PresentationCode,
                MedicineGroupCode = x.MedicineGroupCode,
                Atc = x.Atc,
                Invima = x.Invima,
                Price = x.Price,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,

                MeasurementUnitDescription = x.MeasurementUnit != null
                    ? $"{x.MeasurementUnit.UnitCode} - {x.MeasurementUnit.Description}"
                    : null,
                AdministrationRouteDescription = x.AdministrationRoute?.Description,
                PharmaceuticalFormDescription = x.PharmaceuticalForm?.Description,
                PresentationDescription = x.Presentation?.Description,
                MedicineGroupDescription = x.MedicineGroup?.Description
            });
        }

        [HttpPost]
        public async Task<ActionResult<MedicineResponse>> Create([FromBody] MedicineCreateRequest request)
        {
            // Validación mínima (si ya usas FluentValidation, esto lo mueves allá)
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Name is required.");

            var entity = new Medicine
            {
                Name = request.Name.Trim(),
                Cum = request.Cum?.Trim(),
                Concentration = request.Concentration?.Trim(),
                MeasurementUnitSidamId = request.MeasurementUnitSidamId,
                AdministrationRouteCode = request.AdministrationRouteCode,
                PharmaceuticalFormCode = request.PharmaceuticalFormCode,
                PresentationCode = request.PresentationCode,
                MedicineGroupCode = request.MedicineGroupCode,
                Atc = request.Atc?.Trim(),
                Invima = request.Invima?.Trim(),
                Price = request.Price,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Medicines.Add(entity);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, new MedicineResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Cum = entity.Cum,
                Concentration = entity.Concentration,
                MeasurementUnitSidamId = entity.MeasurementUnitSidamId,
                AdministrationRouteCode = entity.AdministrationRouteCode,
                PharmaceuticalFormCode = entity.PharmaceuticalFormCode,
                PresentationCode = entity.PresentationCode,
                MedicineGroupCode = entity.MedicineGroupCode,
                Atc = entity.Atc,
                Invima = entity.Invima,
                Price = entity.Price,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] MedicineUpdateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Name is required.");

            var entity = await _db.Medicines.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            entity.Name = request.Name.Trim();
            entity.Cum = request.Cum?.Trim();
            entity.Concentration = request.Concentration?.Trim();

            entity.MeasurementUnitSidamId = request.MeasurementUnitSidamId;
            entity.AdministrationRouteCode = request.AdministrationRouteCode;
            entity.PharmaceuticalFormCode = request.PharmaceuticalFormCode;
            entity.PresentationCode = request.PresentationCode;
            entity.MedicineGroupCode = request.MedicineGroupCode;

            entity.Atc = request.Atc?.Trim();
            entity.Invima = request.Invima?.Trim();
            entity.Price = request.Price;

            entity.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _db.Medicines.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            _db.Medicines.Remove(entity);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // 1) Measurement Units
        [HttpGet("measurement-units")]
        public async Task<ActionResult<List<MeasurementUnit>>> GetMeasurementUnits()
        {
            var items = await _db.MeasurementUnits
                .AsNoTracking()
                .OrderBy(x => x.UnitCode)
                .ToListAsync();

            return Ok(items);
        }

        // 2) Administration Routes
        [HttpGet("administration-routes")]
        public async Task<ActionResult<List<AdministrationRoute>>> GetAdministrationRoutes()
        {
            var items = await _db.AdministrationRoutes
                .AsNoTracking()
                .OrderBy(x => x.Code)
                .ToListAsync();

            return Ok(items);
        }

        // 3) Pharmaceutical Forms
        [HttpGet("pharmaceutical-forms")]
        public async Task<ActionResult<List<PharmaceuticalForm>>> GetPharmaceuticalForms()
        {
            var items = await _db.PharmaceuticalForms
                .AsNoTracking()
                .OrderBy(x => x.Code)
                .ToListAsync();

            return Ok(items);
        }

        // 4) Presentations
        [HttpGet("presentations")]
        public async Task<ActionResult<List<Presentation>>> GetPresentations()
        {
            var items = await _db.Presentations
                .AsNoTracking()
                .OrderBy(x => x.Code)
                .ToListAsync();

            return Ok(items);
        }

        // 5) Medicine Groups
        [HttpGet("medicine-groups")]
        public async Task<ActionResult<List<MedicineGroup>>> GetMedicineGroups()
        {
            var items = await _db.MedicineGroups
                .AsNoTracking()
                .OrderBy(x => x.Code)
                .ToListAsync();

            return Ok(items);
        }
    }
}
