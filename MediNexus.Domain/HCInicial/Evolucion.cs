using System;
using MediNexus.Domain.Admissions;

namespace MediNexus.Domain.HCInicial
{
    public class Evolucion
    {
        public int Id { get; set; }
        public int AdmissionId { get; set; }

        public string MotivoConsulta { get; set; } = null!;

        public string? TensionArterial { get; set; }
        public int? FrecuenciaCardiaca { get; set; }
        public int? FrecuenciaRespiratoria { get; set; }
        public decimal? Temperatura { get; set; }
        public int? SaturacionOxigeno { get; set; }
        public int? Glasgow { get; set; }
        public decimal? Peso { get; set; }
        public decimal? Talla { get; set; }
        public decimal? IMC { get; set; }

        public string Plan { get; set; } = null!;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Admission Admission { get; set; } = null!;
    }
}
