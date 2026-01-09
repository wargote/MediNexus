using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediNexus.Domain.Users
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;   // Administrador, Médico, Enfermero, etc.
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public int UserRoleId { get; set; }
        public UserRole UserRole { get; set; } = null!;
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
