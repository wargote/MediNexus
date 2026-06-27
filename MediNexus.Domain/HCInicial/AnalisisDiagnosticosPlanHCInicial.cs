using System;
using System.Collections.Generic;

namespace MediNexus.Domain.HCInicial
{
    public class AnalisisDiagnosticosPlanHCInicial
    {
        public int Id { get; set; }

        public string? Analisis { get; set; }
        public string? Plan { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<AnalisisDiagnosticosPlanHCInicialDiagnostico> Diagnosticos { get; set; } = new List<AnalisisDiagnosticosPlanHCInicialDiagnostico>();
    }
}
