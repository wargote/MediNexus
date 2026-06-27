using System;

namespace MediNexus.Api.Contracts.HCInicial
{
    public class HCInicialResponse
    {
        public int Id { get; set; }
        public int AdmissionId { get; set; }
        public string NombrePaciente { get; set; } = null!;
        public string DocumentoPaciente { get; set; } = null!;
        public int? IdSubjetivoHCInicial { get; set; }
        public SubjetivoHCInicialResponse? Subjetivo { get; set; }
        public int? IdObjetivoHCInicial { get; set; }
        public ObjetivoHCInicialResponse? Objetivo { get; set; }
        public int? IdSignosVitalesHCInicial { get; set; }
        public SignosVitalesHCInicialResponse? SignosVitales { get; set; }
        public int? IdAnalisisDiagnosticosPlanHCInicial { get; set; }
        public AnalisisDiagnosticosPlanHCInicialResponse? AnalisisDiagnosticosPlan { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
