using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.HCInicial
{
    public class SignosVitalesHCInicialCreateRequest
    {
        [MaxLength(20)]
        public string? TensionArterial { get; set; }

        [Range(0, 300, ErrorMessage = "FrecuenciaCardiaca debe estar entre 0 y 300.")]
        public int? FrecuenciaCardiaca { get; set; }

        [Range(0, 100, ErrorMessage = "FrecuenciaRespiratoria debe estar entre 0 y 100.")]
        public int? FrecuenciaRespiratoria { get; set; }

        [Range(0.0, 50.0, ErrorMessage = "Temperatura debe estar entre 0 y 50.")]
        public decimal? Temperatura { get; set; }

        [Range(0, 100, ErrorMessage = "SaturacionOxigeno debe estar entre 0 y 100.")]
        public int? SaturacionOxigeno { get; set; }

        [Range(0, 15, ErrorMessage = "Glasgow debe estar entre 0 y 15.")]
        public int? Glasgow { get; set; }

        [Range(0.0, 500.0, ErrorMessage = "Peso debe estar entre 0 y 500.")]
        public decimal? Peso { get; set; }

        [Range(0.0, 3.0, ErrorMessage = "Talla debe estar entre 0 y 3.")]
        public decimal? Talla { get; set; }
    }
}
