using System;
using MediNexus.Domain.Contracts;
using MediNexus.Domain.ParametersContracts;

namespace MediNexus.Domain.Tariffs
{
    public class Tariff
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ValueMethodId { get; set; }
        public int ContractId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public ValueMethod ValueMethod { get; set; } = null!;
        public Contract Contract { get; set; } = null!;
    }
}
