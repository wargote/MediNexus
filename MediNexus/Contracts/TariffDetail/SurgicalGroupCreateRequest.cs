using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.TariffDetail
{
    public class SurgicalGroupCreateRequest
    {
        [Required]
        [MaxLength(100)]
        public string ReferenceCode { get; set; } = null!;
    }
}
