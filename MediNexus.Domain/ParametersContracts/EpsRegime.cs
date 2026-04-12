using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediNexus.Domain.ParametersContracts
{
    public class EpsRegime
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
    }
}
