using System;
using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.Triage
{
    public class TriageUpdateRequest
    {
        [Required]
        public DateTime FechaHora { get; set; }

        [Required]
        [RegularExpression("^(I|II|III|IV|V)$", ErrorMessage = "Prioridad debe ser I, II, III, IV o V")]
        public string Prioridad { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string MotivoConsulta { get; set; } = null!;

        public VitalSigns SignosVitales { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
