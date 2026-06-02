using System;

namespace MediNexus.Domain.Medicines
{
    public class MedicalDevice
    {
        public int Id { get; set; }
        public string ElementName { get; set; } = null!;
        public int ElementTypeId { get; set; }
        public int ElementUsageId { get; set; }
        public string RipsCode { get; set; } = null!;
        public bool IsReusable { get; set; }
        public bool IsInvasive { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ElementType ElementType { get; set; } = null!;
        public ElementUsage ElementUsage { get; set; } = null!;
    }
}
