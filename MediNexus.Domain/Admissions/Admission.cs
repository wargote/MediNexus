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

        // Parámetros de Admisión (almacenados como texto según el request body del task)
        public string ModalidadAtencion { get; set; } = null!;
        public string MotivoAtencion { get; set; } = null!;
        public string ClasificacionServicio { get; set; } = null!;
        public string GrupoServicio { get; set; } = null!;
        public string Ingreso { get; set; } = null!;
        public string AmbitoAtencion { get; set; } = null!;
        public string FinalidadAtencion { get; set; } = null!;

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
