using System;

namespace MediNexus.Api.Contracts.HCInicial
{
    public class SubjetivoHCInicialResponse
    {
        public int Id { get; set; }
        public string MotivoConsulta { get; set; } = null!;
        public string EnfermedadActual { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
