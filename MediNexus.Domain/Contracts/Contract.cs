using MediNexus.Domain.ParametersContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediNexus.Domain.Insurers;

namespace MediNexus.Domain.Contracts
{
    public class Contract
    {
        public int Id { get; set; }

        public int InsurerId { get; set; }
        public string ContractNumber { get; set; } = null!;
        public string ContractName { get; set; } = null!;

        public int ValueMethodId { get; set; }
        public int BenefitPlanContractTypeId { get; set; }
        public int EpsRegimeId { get; set; }
        public int HealthUserTypeId { get; set; }
        public int PaymentModalityId { get; set; }
        public int CoverageId { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool IsActive { get; set; } = true;

        public int ContractStatusId { get; set; }

        public Insurer Insurer { get; set; } = null!;
        public ValueMethod ValueMethod { get; set; } = null!;
        public BenefitPlanContractType BenefitPlanContractType { get; set; } = null!;
        public EpsRegime EpsRegime { get; set; } = null!;
        public HealthUserType HealthUserType { get; set; } = null!;
        public PaymentModality PaymentModality { get; set; } = null!;
        public Coverage Coverage { get; set; } = null!;
        public ContractStatus ContractStatus { get; set; } = null!;
    }
}
