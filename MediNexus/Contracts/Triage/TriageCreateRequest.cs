using System;
using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.Triage
{
    public class TriageCreateRequest
    {
        [Required]
        [MaxLength(20)]
        public string NumeroDocumento { get; set; } = null!;

        [Required]
        public DateTime FechaHora { get; set; }

        [Required]
        [RegularExpression("^(I|II|III|IV|V)$", ErrorMessage = "Prioridad debe ser I, II, III, IV o V")]
        public string Prioridad { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string MotivoConsulta { get; set; } = null!;

        public VitalSigns SignosVitales { get; set; } = null!;
    }

    public class VitalSigns
    {
        [MaxLength(30)]
        public string? TensionArterial { get; set; }
        public int? FrecuenciaCardiaca { get; set; }
        public int? FrecuenciaRespiratoria { get; set; }
        public decimal? Peso { get; set; }
        public decimal? Talla { get; set; }
        public decimal? Temperatura { get; set; }
        public int? Glasgow { get; set; }
    }
}
