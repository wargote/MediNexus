using System;
using System.Collections.Generic;

namespace MediNexus.Api.Contracts.HCInicial
{
    public class AnalisisDiagnosticosPlanHCInicialResponse
    {
        public int Id { get; set; }
        public string? Analisis { get; set; }
        public string? Plan { get; set; }
        public List<Cie10CodeResponse> Diagnosticos { get; set; } = new();
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
