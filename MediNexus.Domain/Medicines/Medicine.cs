using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediNexus.Domain.Medicines
{
    public class Medicine
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Cum { get; set; }
        public string? Concentration { get; set; }
        public string? MeasurementUnitSidamId { get; set; }
        public string? AdministrationRouteCode { get; set; }
        public string? PharmaceuticalFormCode { get; set; }
        public string? PresentationCode { get; set; }
        public string? MedicineGroupCode { get; set; }
        public string? Atc { get; set; }
        public string? Invima { get; set; }
        public decimal? Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public MeasurementUnit? MeasurementUnit { get; set; }
        public AdministrationRoute? AdministrationRoute { get; set; }
        public PharmaceuticalForm? PharmaceuticalForm { get; set; }
        public Presentation? Presentation { get; set; }
        public MedicineGroup? MedicineGroup { get; set; }
    }
}
