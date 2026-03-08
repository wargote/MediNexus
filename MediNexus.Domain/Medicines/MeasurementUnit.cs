using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediNexus.Domain.Medicines
{
    public class MeasurementUnit
    {
        // PK: '0005', '0257', etc.
        public string SidamId { get; set; } = null!;

        // Unidad de medida del principio activo: 'mg', 'ml', 'µg', etc.
        public string UnitCode { get; set; } = null!;

        public string Description { get; set; } = null!;
        public bool MipresEnabled { get; set; }
        public decimal MipresVersion { get; set; } // 1.0, 2.3, 3.0
        public DateTime EffectiveDate { get; set; }
    }
}
