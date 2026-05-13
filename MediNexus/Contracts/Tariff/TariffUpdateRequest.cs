using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.Tariff
{
    public class TariffUpdateRequest
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [Required]
        public int ValueMethodId { get; set; }

        [Required]
        public int ContractId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
