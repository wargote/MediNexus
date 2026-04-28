using System;
using System.Collections.Generic;

namespace MediNexus.Domain.Admissions.ParametersAdmission
{
    public class ServiceClassification
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<ServiceGroup> ServiceGroups { get; set; } = new List<ServiceGroup>();
    }
}
