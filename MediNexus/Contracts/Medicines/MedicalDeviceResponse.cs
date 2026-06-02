using System;

namespace MediNexus.Api.Contracts.Medicines
{
    public class MedicalDeviceResponse
    {
        public int Id { get; set; }
        public string ElementName { get; set; } = null!;
        public int ElementTypeId { get; set; }
        public string? ElementTypeName { get; set; }
        public int ElementUsageId { get; set; }
        public string? ElementUsageName { get; set; }
        public string RipsCode { get; set; } = null!;
        public bool IsReusable { get; set; }
        public bool IsInvasive { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
