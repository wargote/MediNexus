using System;

namespace MediNexus.Api.Contracts.HCInicial
{
    public class SignosVitalesHCInicialResponse
    {
        public int Id { get; set; }

        public string? TensionArterial { get; set; }
        public int? FrecuenciaCardiaca { get; set; }
        public int? FrecuenciaRespiratoria { get; set; }
        public decimal? Temperatura { get; set; }
        public int? SaturacionOxigeno { get; set; }
        public int? Glasgow { get; set; }
        public decimal? Peso { get; set; }
        public decimal? Talla { get; set; }
        public decimal? IMC { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
