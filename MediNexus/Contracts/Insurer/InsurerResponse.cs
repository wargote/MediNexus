namespace MediNexus.Api.Contracts.Insurer
{
    public class InsurerResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Nit { get; set; } = null!;
        public short? VerificationDigit { get; set; }
        public string? Code { get; set; }
        public string? Address { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }

        public int CityId { get; set; }
        public string CityName { get; set; } = null!;
        public int StateId { get; set; }
        public string StateName { get; set; } = null!;
        public int CountryId { get; set; }
        public string CountryName { get; set; } = null!;

        public int AdministratorTypeId { get; set; }
        public string AdministratorTypeName { get; set; } = null!;
        public string? AdministratorTypeShortName { get; set; }
    }

    public class InsurerCreateRequest
    {
        public string Name { get; set; } = null!;
        public string Nit { get; set; } = null!;
        public short? VerificationDigit { get; set; }
        public string? Code { get; set; }
        public string? Address { get; set; }
        public int CityId { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }
        public int AdministratorTypeId { get; set; }
    }

    public class InsurerUpdateRequest
    {
        public string Name { get; set; } = null!;
        public string Nit { get; set; } = null!;
        public short? VerificationDigit { get; set; }
        public string? Code { get; set; }
        public string? Address { get; set; }
        public int CityId { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }
        public int AdministratorTypeId { get; set; }
        public bool IsActive { get; set; }  // por si quieres reactivar
    }
}
