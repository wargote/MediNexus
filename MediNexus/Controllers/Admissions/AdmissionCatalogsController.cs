using MediNexus.Api.Contracts.Admissions;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers.Admissions
{
    [Route("api/admissions/catalogs")]
    [ApiController]
    [Authorize]
    public class AdmissionCatalogsController : ControllerBase
    {
        private readonly MediNexusDbContext _context;

        public AdmissionCatalogsController(MediNexusDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(AdmissionCatalogsResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCatalogs()
        {
            var response = new AdmissionCatalogsResponseDto
            {
                CareModalities = await _context.CareModalities
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.Name)
                    .Select(x => new AdmissionCatalogItemDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IsActive = x.IsActive
                    })
                    .ToListAsync(),

                CareScopes = await _context.CareScopes
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.Name)
                    .Select(x => new AdmissionCatalogItemDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IsActive = x.IsActive
                    })
                    .ToListAsync(),

                CareReasons = await _context.CareReasons
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.Name)
                    .Select(x => new AdmissionCatalogItemDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IsActive = x.IsActive
                    })
                    .ToListAsync(),

                AdmissionTypes = await _context.AdmissionTypes
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.Name)
                    .Select(x => new AdmissionCatalogItemDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IsActive = x.IsActive
                    })
                    .ToListAsync(),

                CarePurposes = await _context.CarePurposes
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.Name)
                    .Select(x => new AdmissionCatalogItemDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IsActive = x.IsActive
                    })
                    .ToListAsync(),

                ServiceClassifications = await _context.ServiceClassifications
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.Name)
                    .Select(x => new AdmissionCatalogItemDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IsActive = x.IsActive
                    })
                    .ToListAsync(),

                ServiceGroups = await _context.ServiceGroups
                    .AsNoTracking()
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.ServiceClassification.Name).ThenBy(x => x.Name)
                    .Select(x => new ServiceGroupDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ServiceClassificationId = x.ServiceClassificationId,
                        ServiceClassificationName = x.ServiceClassification.Name,
                        IsActive = x.IsActive
                    })
                    .ToListAsync()
            };

            return Ok(response);
        }

        [HttpGet("care-modalities")]
        [ProducesResponseType(typeof(IEnumerable<AdmissionCatalogItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCareModalities()
        {
            var items = await _context.CareModalities
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new AdmissionCatalogItemDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("care-scopes")]
        [ProducesResponseType(typeof(IEnumerable<AdmissionCatalogItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCareScopes()
        {
            var items = await _context.CareScopes
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new AdmissionCatalogItemDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("care-reasons")]
        [ProducesResponseType(typeof(IEnumerable<AdmissionCatalogItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCareReasons()
        {
            var items = await _context.CareReasons
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new AdmissionCatalogItemDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("admission-types")]
        [ProducesResponseType(typeof(IEnumerable<AdmissionCatalogItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAdmissionTypes()
        {
            var items = await _context.AdmissionTypes
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new AdmissionCatalogItemDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("care-purposes")]
        [ProducesResponseType(typeof(IEnumerable<AdmissionCatalogItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCarePurposes()
        {
            var items = await _context.CarePurposes
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new AdmissionCatalogItemDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("service-classifications")]
        [ProducesResponseType(typeof(IEnumerable<AdmissionCatalogItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetServiceClassifications()
        {
            var items = await _context.ServiceClassifications
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => new AdmissionCatalogItemDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("service-groups")]
        [ProducesResponseType(typeof(IEnumerable<ServiceGroupDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetServiceGroups([FromQuery] int? classificationId)
        {
            var query = _context.ServiceGroups
                .AsNoTracking()
                .Where(x => x.IsActive);

            if (classificationId.HasValue)
                query = query.Where(x => x.ServiceClassificationId == classificationId.Value);

            var items = await query
                .OrderBy(x => x.ServiceClassification.Name).ThenBy(x => x.Name)
                .Select(x => new ServiceGroupDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    ServiceClassificationId = x.ServiceClassificationId,
                    ServiceClassificationName = x.ServiceClassification.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Ok(items);
        }
    }
}
