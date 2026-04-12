using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.Contracts
{
    public class CreateContractRequest
    {
        [Required]
        public int InsurerId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ContractNumber { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string ContractName { get; set; } = null!;

        [Required]
        public int ValueMethodId { get; set; }

        [Required]
        public int BenefitPlanContractTypeId { get; set; }

        [Required]
        public int EpsRegimeId { get; set; }

        [Required]
        public int HealthUserTypeId { get; set; }

        [Required]
        public int PaymentModalityId { get; set; }

        [Required]
        public int CoverageId { get; set; }

        [Required]
        public DateOnly StartDate { get; set; }

        [Required]
        public DateOnly EndDate { get; set; }

        [Required]
        public int ContractStatusId { get; set; }
    }
}
