using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediNexus.Domain.Medicines
{
    public class PharmaceuticalForm
    {
        public string Code { get; set; } = null!;

        public string Description { get; set; } = null!;
        public bool MipresEnabled { get; set; }
        public decimal MipresVersion { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
