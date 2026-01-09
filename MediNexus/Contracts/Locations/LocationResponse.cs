namespace MediNexus.Api.Contracts.Locations
{
    public class CountryResponse
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
    }

    public class StateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? DaneCode { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; } = null!;
    }

    public class CityResponse
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string? FuripsCode { get; set; }
        public string Name { get; set; } = null!;
        public int StateId { get; set; }
        public string StateName { get; set; } = null!;
        public int CountryId { get; set; }
        public string CountryName { get; set; } = null!;
    }
}
