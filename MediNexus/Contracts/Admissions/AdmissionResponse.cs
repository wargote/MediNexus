using System;

namespace MediNexus.Api.Contracts.Admissions
{
    public class AdmissionResponse
    {
        public int Id { get; set; }

        // Paciente
        public int PatientId { get; set; }
        public string DocumentoPatiente { get; set; } = null!;
        public string NombrePaciente { get; set; } = null!;

        // Triage asociado
        public int TriageId { get; set; }
        public string TriagePrioridad { get; set; } = null!;
        public DateTime TriageFechaHora { get; set; }

        // Datos de admisión
        public int CareModalityId { get; set; }
        public string CareModalityName { get; set; } = null!;

        public int CareReasonId { get; set; }
        public string CareReasonName { get; set; } = null!;

        public int ServiceClassificationId { get; set; }
        public string ServiceClassificationName { get; set; } = null!;

        public int ServiceGroupId { get; set; }
        public string ServiceGroupName { get; set; } = null!;

        public int AdmissionTypeId { get; set; }
        public string AdmissionTypeName { get; set; } = null!;

        public int CareScopeId { get; set; }
        public string CareScopeName { get; set; } = null!;

        public int CarePurposeId { get; set; }
        public string CarePurposeName { get; set; } = null!;

        // EPS y Convenio
        public int EpsId { get; set; }
        public string EpsNombre { get; set; } = null!;
        public int ConvenioId { get; set; }
        public string ConvenioNombre { get; set; } = null!;

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
