using System;

namespace MediNexus.Api.Contracts.HCInicial
{
    public class Cie10CodeResponse
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
