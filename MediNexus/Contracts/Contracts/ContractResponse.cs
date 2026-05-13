namespace MediNexus.Api.Contracts.Contracts
{
    public class ContractResponse
    {
        public int Id { get; set; }

        public int InsurerId { get; set; }
        public string InsurerName { get; set; } = null!;

        public string ContractNumber { get; set; } = null!;
        public string ContractName { get; set; } = null!;

        public int ValueMethodId { get; set; }
        public string ValueMethodDescription { get; set; } = null!;

        public int BenefitPlanContractTypeId { get; set; }
        public string BenefitPlanContractTypeDescription { get; set; } = null!;

        public int EpsRegimeId { get; set; }
        public string EpsRegimeDescription { get; set; } = null!;

        public int HealthUserTypeId { get; set; }
        public string HealthUserTypeDescription { get; set; } = null!;

        public int PaymentModalityId { get; set; }
        public string PaymentModalityDescription { get; set; } = null!;

        public int CoverageId { get; set; }
        public string CoverageDescription { get; set; } = null!;

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public int ContractStatusId { get; set; }
        public string ContractStatusDescription { get; set; } = null!;
    }
    public class ContractListResponse
    {
        public int Id { get; set; }
        public string ContractNumber { get; set; } = null!;
        public string ContractName { get; set; } = null!;
        public string InsurerName { get; set; } = null!;
        public string ContractStatusDescription { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }

    public class ContractCandidateResponse
    {
        public int Id { get; set; }
        public string ContractNumber { get; set; } = null!;
        public string ContractName { get; set; } = null!;
        public string ContractStatusDescription { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
