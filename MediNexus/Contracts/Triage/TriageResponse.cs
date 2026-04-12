using MediNexus.Api.Contracts.Triage;
using System;

namespace MediNexus.Api.Contracts.Triage
{
    public class TriageResponse
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string NumeroDocumento { get; set; } = null!;
        public string NombrePaciente { get; set; } = null!;
        public DateTime FechaHora { get; set; }
        public string Prioridad { get; set; } = null!;
        public string MotivoConsulta { get; set; } = null!;
        public VitalSigns SignosVitales { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
