using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediNexus.Domain.Administrator
{
    public class AdministratorType
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;      // tipo_administradora
        public string? ShortName { get; set; }        // contracción
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ICollection<Insurer.Insurer> Insurers { get; set; } = new List<Insurer.Insurer>();
    }
}
