using System;

namespace MediNexus.Api.Contracts.HCInicial
{
    public class EvolucionEspecialistaResponse
    {
        public int Id { get; set; }
        public int AdmissionId { get; set; }
        public string MotivoConsulta { get; set; } = null!;
        public string Plan { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
