using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.HCInicial
{
    public class EvolucionCreateRequest
    {
        [Required(ErrorMessage = "El ID de admisión es obligatorio.")]
        public int AdmissionId { get; set; }

        [Required(ErrorMessage = "El motivo de consulta es obligatorio.")]
        [MaxLength(1000, ErrorMessage = "El motivo de consulta no puede superar 1000 caracteres.")]
        public string MotivoConsulta { get; set; } = null!;

        [MaxLength(20, ErrorMessage = "La tensión arterial no puede superar 20 caracteres.")]
        public string? TensionArterial { get; set; }

        public int? FrecuenciaCardiaca { get; set; }

        public int? FrecuenciaRespiratoria { get; set; }

        public decimal? Temperatura { get; set; }

        public int? SaturacionOxigeno { get; set; }

        public int? Glasgow { get; set; }

        public decimal? Peso { get; set; }

        public decimal? Talla { get; set; }

        [Required(ErrorMessage = "El plan médico es obligatorio.")]
        [MaxLength(1000, ErrorMessage = "El plan no puede superar 1000 caracteres.")]
        public string Plan { get; set; } = null!;
    }
}
