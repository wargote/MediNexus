namespace MediNexus.Api.Contracts.ParametersContracts
{
    public class CatalogItemDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
    }
    public class ContractCatalogsResponseDto
    {
        public List<CatalogItemDto> ValueMethods { get; set; } = [];
        public List<CatalogItemDto> BenefitPlanContractTypes { get; set; } = [];
        public List<CatalogItemDto> EpsRegimes { get; set; } = [];
        public List<CatalogItemDto> PaymentModalities { get; set; } = [];
        public List<CatalogItemDto> HealthUserTypes { get; set; } = [];
        public List<CatalogItemDto> Coverages { get; set; } = [];
    }
}
