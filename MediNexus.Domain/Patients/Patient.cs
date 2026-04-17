using MediNexus.Domain.Administrator;
using MediNexus.Domain.Insurers;
using MediNexus.Domain.Location;
using MediNexus.Domain.Users;
using System;

namespace MediNexus.Domain.Patients
{
    public class Patient
    {
        public int Id { get; set; }
        public int InsurerId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string? SecondLastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int? SexId { get; set; }
        public int BirthCountryId { get; set; }
        public int ResidenceCountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public int? ZoneId { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int? MaritalStatusId { get; set; }
        public int? DisabilityId { get; set; }
        public int? BloodGroupId { get; set; }
        public int? RhFactorId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public Insurer Insurer { get; set; } = null!;
        public DocumentType DocumentType { get; set; } = null!;
        public Country BirthCountry { get; set; } = null!;
        public Country ResidenceCountry { get; set; } = null!;
        public State State { get; set; } = null!;
        public City City { get; set; } = null!;

        public Sex? Sex { get; set; }
        public Zone? Zone { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public Disability? Disability { get; set; }
        public BloodGroup? BloodGroup { get; set; }
        public RhFactor? RhFactor { get; set; }
    }
}
