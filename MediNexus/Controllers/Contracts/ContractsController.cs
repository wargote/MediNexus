using MediNexus.Api.Contracts.Contracts;
using MediNexus.Domain.Contracts;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers.Contracts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContractsController : ControllerBase
    {
        private readonly MediNexusDbContext _context;

        public ContractsController(MediNexusDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ContractListResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var contracts = await _context.Contracts
                .AsNoTracking()
                .Include(x => x.Insurer)
                .Include(x => x.ContractStatus)
                .OrderByDescending(x => x.Id)
                .Select(x => new ContractListResponse
                {
                    Id = x.Id,
                    ContractNumber = x.ContractNumber,
                    ContractName = x.ContractName,
                    InsurerName = x.Insurer.Name,
                    ContractStatusDescription = x.ContractStatus.Description,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                })
                .ToListAsync();

            return Ok(contracts);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ContractResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var contract = await _context.Contracts
                .AsNoTracking()
                .Include(x => x.Insurer)
                .Include(x => x.ValueMethod)
                .Include(x => x.BenefitPlanContractType)
                .Include(x => x.EpsRegime)
                .Include(x => x.HealthUserType)
                .Include(x => x.PaymentModality)
                .Include(x => x.Coverage)
                .Include(x => x.ContractStatus)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (contract == null)
                return NotFound(new { message = "Contrato no encontrado." });

            var response = new ContractResponse
            {
                Id = contract.Id,
                InsurerId = contract.InsurerId,
                InsurerName = contract.Insurer.Name,
                ContractNumber = contract.ContractNumber,
                ContractName = contract.ContractName,
                ValueMethodId = contract.ValueMethodId,
                ValueMethodDescription = contract.ValueMethod.Description,
                BenefitPlanContractTypeId = contract.BenefitPlanContractTypeId,
                BenefitPlanContractTypeDescription = contract.BenefitPlanContractType.Description,
                EpsRegimeId = contract.EpsRegimeId,
                EpsRegimeDescription = contract.EpsRegime.Description,
                HealthUserTypeId = contract.HealthUserTypeId,
                HealthUserTypeDescription = contract.HealthUserType.Description,
                PaymentModalityId = contract.PaymentModalityId,
                PaymentModalityDescription = contract.PaymentModality.Description,
                CoverageId = contract.CoverageId,
                CoverageDescription = contract.Coverage.Description,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                ContractStatusId = contract.ContractStatusId,
                ContractStatusDescription = contract.ContractStatus.Description
            };

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ContractResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateContractRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.StartDate > request.EndDate)
                return BadRequest(new { message = "La fecha inicial no puede ser mayor a la fecha de terminación." });

            var foreignKeysAreValid = await ValidateForeignKeys(
                request.InsurerId,
                request.ValueMethodId,
                request.BenefitPlanContractTypeId,
                request.EpsRegimeId,
                request.HealthUserTypeId,
                request.PaymentModalityId,
                request.CoverageId,
                request.ContractStatusId);

            if (!foreignKeysAreValid.IsValid)
                return BadRequest(new { message = foreignKeysAreValid.Message });

            var duplicateExists = await _context.Contracts.AnyAsync(x =>
                x.InsurerId == request.InsurerId &&
                x.ContractNumber == request.ContractNumber);

            if (duplicateExists)
                return BadRequest(new { message = "Ya existe un contrato con ese número para la aseguradora seleccionada." });

            var contract = new Contract
            {
                InsurerId = request.InsurerId,
                ContractNumber = request.ContractNumber.Trim(),
                ContractName = request.ContractName.Trim(),
                ValueMethodId = request.ValueMethodId,
                BenefitPlanContractTypeId = request.BenefitPlanContractTypeId,
                EpsRegimeId = request.EpsRegimeId,
                HealthUserTypeId = request.HealthUserTypeId,
                PaymentModalityId = request.PaymentModalityId,
                CoverageId = request.CoverageId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ContractStatusId = request.ContractStatusId
            };

            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();

            var response = await BuildContractResponse(contract.Id);

            return CreatedAtAction(nameof(GetById), new { id = contract.Id }, response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ContractResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateContractRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.StartDate > request.EndDate)
                return BadRequest(new { message = "La fecha inicial no puede ser mayor a la fecha de terminación." });

            var contract = await _context.Contracts.FirstOrDefaultAsync(x => x.Id == id);

            if (contract == null)
                return NotFound(new { message = "Contrato no encontrado." });

            var foreignKeysAreValid = await ValidateForeignKeys(
                request.InsurerId,
                request.ValueMethodId,
                request.BenefitPlanContractTypeId,
                request.EpsRegimeId,
                request.HealthUserTypeId,
                request.PaymentModalityId,
                request.CoverageId,
                request.ContractStatusId);

            if (!foreignKeysAreValid.IsValid)
                return BadRequest(new { message = foreignKeysAreValid.Message });

            var duplicateExists = await _context.Contracts.AnyAsync(x =>
                x.Id != id &&
                x.InsurerId == request.InsurerId &&
                x.ContractNumber == request.ContractNumber);

            if (duplicateExists)
                return BadRequest(new { message = "Ya existe otro contrato con ese número para la aseguradora seleccionada." });

            contract.InsurerId = request.InsurerId;
            contract.ContractNumber = request.ContractNumber.Trim();
            contract.ContractName = request.ContractName.Trim();
            contract.ValueMethodId = request.ValueMethodId;
            contract.BenefitPlanContractTypeId = request.BenefitPlanContractTypeId;
            contract.EpsRegimeId = request.EpsRegimeId;
            contract.HealthUserTypeId = request.HealthUserTypeId;
            contract.PaymentModalityId = request.PaymentModalityId;
            contract.CoverageId = request.CoverageId;
            contract.StartDate = request.StartDate;
            contract.EndDate = request.EndDate;
            contract.ContractStatusId = request.ContractStatusId;

            await _context.SaveChangesAsync();

            var response = await BuildContractResponse(contract.Id);

            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var contract = await _context.Contracts.FirstOrDefaultAsync(x => x.Id == id);

            if (contract == null)
                return NotFound(new { message = "Contrato no encontrado." });

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Contrato eliminado correctamente." });
        }

        private async Task<(bool IsValid, string Message)> ValidateForeignKeys(
            int insurerId,
            int valueMethodId,
            int benefitPlanContractTypeId,
            int epsRegimeId,
            int healthUserTypeId,
            int paymentModalityId,
            int coverageId,
            int contractStatusId)
        {
            if (!await _context.Insurers.AnyAsync(x => x.Id == insurerId))
                return (false, "La aseguradora seleccionada no existe.");

            if (!await _context.ValueMethods.AnyAsync(x => x.Id == valueMethodId))
                return (false, "El método de valores seleccionado no existe.");

            if (!await _context.BenefitPlanContractTypes.AnyAsync(x => x.Id == benefitPlanContractTypeId))
                return (false, "El tipo de contrato seleccionado no existe.");

            if (!await _context.EpsRegimes.AnyAsync(x => x.Id == epsRegimeId))
                return (false, "El régimen EPS seleccionado no existe.");

            if (!await _context.HealthUserTypes.AnyAsync(x => x.Id == healthUserTypeId))
                return (false, "El tipo de usuario seleccionado no existe.");

            if (!await _context.PaymentModalities.AnyAsync(x => x.Id == paymentModalityId))
                return (false, "La modalidad de pago seleccionada no existe.");

            if (!await _context.Coverages.AnyAsync(x => x.Id == coverageId))
                return (false, "La cobertura seleccionada no existe.");

            if (!await _context.ContractStatuses.AnyAsync(x => x.Id == contractStatusId))
                return (false, "El estado del contrato seleccionado no existe.");

            return (true, string.Empty);
        }

        private async Task<ContractResponse> BuildContractResponse(int contractId)
        {
            var contract = await _context.Contracts
                .AsNoTracking()
                .Include(x => x.Insurer)
                .Include(x => x.ValueMethod)
                .Include(x => x.BenefitPlanContractType)
                .Include(x => x.EpsRegime)
                .Include(x => x.HealthUserType)
                .Include(x => x.PaymentModality)
                .Include(x => x.Coverage)
                .Include(x => x.ContractStatus)
                .FirstAsync(x => x.Id == contractId);

            return new ContractResponse
            {
                Id = contract.Id,
                InsurerId = contract.InsurerId,
                InsurerName = contract.Insurer.Name,
                ContractNumber = contract.ContractNumber,
                ContractName = contract.ContractName,
                ValueMethodId = contract.ValueMethodId,
                ValueMethodDescription = contract.ValueMethod.Description,
                BenefitPlanContractTypeId = contract.BenefitPlanContractTypeId,
                BenefitPlanContractTypeDescription = contract.BenefitPlanContractType.Description,
                EpsRegimeId = contract.EpsRegimeId,
                EpsRegimeDescription = contract.EpsRegime.Description,
                HealthUserTypeId = contract.HealthUserTypeId,
                HealthUserTypeDescription = contract.HealthUserType.Description,
                PaymentModalityId = contract.PaymentModalityId,
                PaymentModalityDescription = contract.PaymentModality.Description,
                CoverageId = contract.CoverageId,
                CoverageDescription = contract.Coverage.Description,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                ContractStatusId = contract.ContractStatusId,
                ContractStatusDescription = contract.ContractStatus.Description
            };
        }
    }
}
