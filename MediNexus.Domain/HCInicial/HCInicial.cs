using System;
using MediNexus.Domain.Admissions;

namespace MediNexus.Domain.HCInicial
{
    public class HCInicial
    {
        public int Id { get; set; }
        public int AdmissionId { get; set; }

        public int? IdSubjetivoHCInicial { get; set; }
        public int? IdObjetivoHCInicial { get; set; }
        public int? IdSignosVitalesHCInicial { get; set; }
        public int? IdAnalisisDiagnosticosPlanHCInicial { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Admission Admission { get; set; } = null!;
        public SubjetivoHCInicial? Subjetivo { get; set; }
        public ObjetivoHCInicial? Objetivo { get; set; }
        public SignosVitalesHCInicial? SignosVitales { get; set; }
        public AnalisisDiagnosticosPlanHCInicial? AnalisisDiagnosticosPlan { get; set; }
    }
}
