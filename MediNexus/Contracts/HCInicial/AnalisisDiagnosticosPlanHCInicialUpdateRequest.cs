using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.HCInicial
{
    public class AnalisisDiagnosticosPlanHCInicialUpdateRequest
    {
        [MaxLength(1000)]
        public string? Analisis { get; set; }

        [MaxLength(1000)]
        public string? Plan { get; set; }

        public List<int> IdsCie10 { get; set; } = new();

        public bool IsActive { get; set; } = true;
    }
}
