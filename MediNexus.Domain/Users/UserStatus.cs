using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediNexus.Domain.Users
{
    public class UserStatus
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;   // ACT, INA, BLO, etc.
        public string Name { get; set; } = null!;   // Activo, Inactivo, Bloqueado
        public bool IsActive { get; set; } = true;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
