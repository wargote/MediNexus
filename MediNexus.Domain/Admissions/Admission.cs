using System;
using MediNexus.Domain.Admissions.ParametersAdmission;
using MediNexus.Domain.Contracts;
using MediNexus.Domain.Insurers;
using MediNexus.Domain.Patients;
using MediNexus.Domain.Triages;

namespace MediNexus.Domain.Admissions
{
    public class Admission
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int TriageId { get; set; }
        public int InsurerId { get; set; }
        public int ConvenioId { get; set; }

        // Parámetros de Admisión
        public int CareModalityId { get; set; }
        public int CareReasonId { get; set; }
        public int ServiceClassificationId { get; set; }
        public int ServiceGroupId { get; set; }
        public int AdmissionTypeId { get; set; }
        public int CareScopeId { get; set; }
        public int CarePurposeId { get; set; }

        public CareModality CareModality { get; set; } = null!;
        public CareReason CareReason { get; set; } = null!;
        public ServiceClassification ServiceClassification { get; set; } = null!;
        public ServiceGroup ServiceGroup { get; set; } = null!;
        public AdmissionType AdmissionType { get; set; } = null!;
        public CareScope CareScope { get; set; } = null!;
        public CarePurpose CarePurpose { get; set; } = null!;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Patient Patient { get; set; } = null!;
        public Triage Triage { get; set; } = null!;
        public Insurer Insurer { get; set; } = null!;
        public Contract Convenio { get; set; } = null!;
    }
}
