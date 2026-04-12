using System;

namespace MediNexus.Api.Contracts.Patient
{
    public class PatientResponse
    {
        public int Id { get; set; }
        public int InsurerId { get; set; }
        public string InsurerName { get; set; } = null!;
        
        public int? ContractId { get; set; }
        public string? ContractName { get; set; } 
        
        public int DocumentTypeId { get; set; }
        public string DocumentTypeCode { get; set; } = null!;
        public string DocumentTypeName { get; set; } = null!;
        
        public string DocumentNumber { get; set; } = null!;
        
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string? SecondLastName { get; set; }
        
        public DateTime BirthDate { get; set; }
        public int? SexId { get; set; }
        public string? SexName { get; set; }
        
        public int BirthCountryId { get; set; }
        public string BirthCountryName { get; set; } = null!;
        
        public int ResidenceCountryId { get; set; }
        public string ResidenceCountryName { get; set; } = null!;
        
        public int StateId { get; set; }
        public string StateName { get; set; } = null!;
        
        public int CityId { get; set; }
        public string CityName { get; set; } = null!;
        
        public int? ZoneId { get; set; }
        public string? ZoneName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        
        public int? MaritalStatusId { get; set; }
        public string? MaritalStatusName { get; set; }
        public int? DisabilityId { get; set; }
        public string? DisabilityName { get; set; }
        public int? BloodGroupId { get; set; }
        public string? BloodGroupName { get; set; }
        public int? RhFactorId { get; set; }
        public string? RhFactorName { get; set; }
        
        public bool IsActive { get; set; }
    }
}
