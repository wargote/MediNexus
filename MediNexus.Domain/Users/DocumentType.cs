using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediNexus.Domain.Users
{
    public class DocumentType
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;   // CC, TI, PAS, etc.
        public string Name { get; set; } = null!;   // Cédula de ciudadanía
        public bool IsActive { get; set; } = true;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
