using System;
using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.Patient
{
    public class PatientUpdateRequest
    {
        [Required]
        public int InsurerId { get; set; }

        [Required]
        public int DocumentTypeId { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string DocumentNumber { get; set; } = null!;
        
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = null!;
        
        [MaxLength(100)]
        public string? MiddleName { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = null!;
        
        [MaxLength(100)]
        public string? SecondLastName { get; set; }
        
        [Required]
        public DateTime BirthDate { get; set; }
        
        [Required]
        public int? SexId { get; set; } 
        
        [Required]
        public int BirthCountryId { get; set; }
        
        [Required]
        public int ResidenceCountryId { get; set; }
        
        [Required]
        public int StateId { get; set; }
        
        [Required]
        public int CityId { get; set; }
        
        public int? ZoneId { get; set; }
        
        [MaxLength(250)]
        public string? Address { get; set; }
        
        [MaxLength(20)]
        public string? Phone { get; set; }
        
        [MaxLength(150)]
        [EmailAddress]
        public string? Email { get; set; }
        
        public int? MaritalStatusId { get; set; }
        
        public int? DisabilityId { get; set; }
        
        public int? BloodGroupId { get; set; }
        
        public int? RhFactorId { get; set; }

        public bool IsActive { get; set; }
    }
}
