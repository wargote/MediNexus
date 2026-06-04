using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.Medicines
{
    public class MedicalDeviceCreateRequest
    {
        [Required]
        [MaxLength(300)]
        public string ElementName { get; set; } = null!;

        [Required]
        public int ElementTypeId { get; set; }

        [Required]
        public int ElementUsageId { get; set; }

        [Required]
        [MaxLength(50)]
        public string RipsCode { get; set; } = null!;

        public bool IsReusable { get; set; }
        public bool IsInvasive { get; set; }
    }
}
