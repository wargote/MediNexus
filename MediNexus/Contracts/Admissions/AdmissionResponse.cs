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
        public string ModalidadAtencion { get; set; } = null!;
        public string MotivoAtencion { get; set; } = null!;
        public string ClasificacionServicio { get; set; } = null!;
        public string GrupoServicio { get; set; } = null!;
        public string Ingreso { get; set; } = null!;
        public string AmbitoAtencion { get; set; } = null!;
        public string FinalidadAtencion { get; set; } = null!;

        // EPS y Convenio
        public int EpsId { get; set; }
        public string EpsNombre { get; set; } = null!;
        public int ConvenioId { get; set; }
        public string ConvenioNombre { get; set; } = null!;

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
