using System;
using MediNexus.Domain.Admissions;

namespace MediNexus.Domain.HCInicial
{
    public class EvolucionEspecialista
    {
        public int Id { get; set; }
        public int AdmissionId { get; set; }

        public string MotivoConsulta { get; set; } = null!;
        public string Plan { get; set; } = null!;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Admission Admission { get; set; } = null!;
    }
}
