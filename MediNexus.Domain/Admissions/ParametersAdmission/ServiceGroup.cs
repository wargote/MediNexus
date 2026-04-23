using System;

namespace MediNexus.Domain.Admissions.ParametersAdmission
{
    public class ServiceGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public int ServiceClassificationId { get; set; }
        public ServiceClassification ServiceClassification { get; set; } = null!;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
