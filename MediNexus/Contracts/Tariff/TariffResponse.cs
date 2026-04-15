using System;

namespace MediNexus.Api.Contracts.Tariff
{
    public class TariffResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ValueMethodId { get; set; }
        public string? ValueMethodDescription { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
