using MediNexus.Api.Contracts.ParametersContracts;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers.Contracts
{
    [Route("api/contracts/catalogs")]
    [ApiController]
    [Authorize]
    public class ContractsCatalogsController : ControllerBase
    {
        private readonly MediNexusDbContext _context;

        public ContractsCatalogsController(MediNexusDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ContractCatalogsResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCatalogs()
        {
            var response = new ContractCatalogsResponseDto
            {
                ValueMethods = await _context.ValueMethods
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .Select(x => new CatalogItemDto
                    {
                        Id = x.Id,
                        Description = x.Description
                    })
                    .ToListAsync(),

                BenefitPlanContractTypes = await _context.BenefitPlanContractTypes
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .Select(x => new CatalogItemDto
                    {
                        Id = x.Id,
                        Description = x.Description
                    })
                    .ToListAsync(),

                EpsRegimes = await _context.EpsRegimes
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .Select(x => new CatalogItemDto
                    {
                        Id = x.Id,
                        Description = x.Description
                    })
                    .ToListAsync(),

                PaymentModalities = await _context.PaymentModalities
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .Select(x => new CatalogItemDto
                    {
                        Id = x.Id,
                        Description = x.Description
                    })
                    .ToListAsync(),

                HealthUserTypes = await _context.HealthUserTypes
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .Select(x => new CatalogItemDto
                    {
                        Id = x.Id,
                        Description = x.Description
                    })
                    .ToListAsync(),

                Coverages = await _context.Coverages
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .Select(x => new CatalogItemDto
                    {
                        Id = x.Id,
                        Description = x.Description
                    })
                    .ToListAsync()
            };

            return Ok(response);
        }
    }
}
