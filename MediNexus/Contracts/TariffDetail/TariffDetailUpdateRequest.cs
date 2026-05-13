using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.TariffDetail
{
    public class TariffDetailUpdateRequest
    {
        [Required]
        public int ReferenceCode { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = null!;

        [Required]
        public decimal Value { get; set; }

        public bool IsSurgicalProcedure { get; set; }

        [Required]
        public decimal Factors { get; set; }

        [Required]
        public int TariffId { get; set; }

        [Required]
        public int SurgicalGroupId { get; set; }
    }
}
