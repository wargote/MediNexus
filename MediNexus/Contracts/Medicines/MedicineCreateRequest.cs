namespace MediNexus.Api.Contracts.Medicines
{
    public class MedicineCreateRequest
    {
        public string Name { get; set; } = null!;
        public string? Cum { get; set; }
        public string? Concentration { get; set; }

        public string? MeasurementUnitSidamId { get; set; }
        public string? AdministrationRouteCode { get; set; }
        public string? PharmaceuticalFormCode { get; set; }
        public string? PresentationCode { get; set; }
        public string? MedicineGroupCode { get; set; }

        public string? Atc { get; set; }
        public string? Invima { get; set; }
        public decimal? Price { get; set; }
    }

    public class MedicineUpdateRequest
    {
        public string Name { get; set; } = null!;
        public string? Cum { get; set; }
        public string? Concentration { get; set; }

        public string? MeasurementUnitSidamId { get; set; }
        public string? AdministrationRouteCode { get; set; }
        public string? PharmaceuticalFormCode { get; set; }
        public string? PresentationCode { get; set; }
        public string? MedicineGroupCode { get; set; }

        public string? Atc { get; set; }
        public string? Invima { get; set; }
        public decimal? Price { get; set; }
    }

    public class MedicineResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Cum { get; set; }
        public string? Concentration { get; set; }

        public string? MeasurementUnitSidamId { get; set; }
        public string? AdministrationRouteCode { get; set; }
        public string? PharmaceuticalFormCode { get; set; }
        public string? PresentationCode { get; set; }
        public string? MedicineGroupCode { get; set; }

        public string? Atc { get; set; }
        public string? Invima { get; set; }
        public decimal? Price { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? MeasurementUnitDescription { get; set; }
        public string? AdministrationRouteDescription { get; set; }
        public string? PharmaceuticalFormDescription { get; set; }
        public string? PresentationDescription { get; set; }
        public string? MedicineGroupDescription { get; set; }
    }

    public class LookupItemResponse
    {
        public string Code { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class MedicineLookupsResponse
    {
        public List<LookupItemResponse> MeasurementUnits { get; set; } = new();
        public List<LookupItemResponse> AdministrationRoutes { get; set; } = new();
        public List<LookupItemResponse> PharmaceuticalForms { get; set; } = new();
        public List<LookupItemResponse> Presentations { get; set; } = new();
        public List<LookupItemResponse> MedicineGroups { get; set; } = new();
    }

}
