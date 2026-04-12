using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediNexus.Domain.Contracts
{
    public class ContractStatus
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    }
}
