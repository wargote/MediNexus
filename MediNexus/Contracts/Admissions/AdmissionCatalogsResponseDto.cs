namespace MediNexus.Api.Contracts.Admissions
{
    public class AdmissionCatalogItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    public class ServiceGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ServiceClassificationId { get; set; }
        public string ServiceClassificationName { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    public class AdmissionCatalogsResponseDto
    {
        public List<AdmissionCatalogItemDto> CareModalities { get; set; } = [];
        public List<AdmissionCatalogItemDto> CareScopes { get; set; } = [];
        public List<AdmissionCatalogItemDto> CareReasons { get; set; } = [];
        public List<AdmissionCatalogItemDto> AdmissionTypes { get; set; } = [];
        public List<AdmissionCatalogItemDto> CarePurposes { get; set; } = [];
        public List<AdmissionCatalogItemDto> ServiceClassifications { get; set; } = [];
        public List<ServiceGroupDto> ServiceGroups { get; set; } = [];
    }
}
