using System;

namespace MediNexus.Domain.HCInicial
{
    public class SubjetivoHCInicial
    {
        public int Id { get; set; }
        public string MotivoConsulta { get; set; } = null!;
        public string EnfermedadActual { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
