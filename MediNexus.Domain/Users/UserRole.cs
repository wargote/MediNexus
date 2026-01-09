using MediNexus.Domain.NavegationMenus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediNexus.Domain.Users
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;   // Admin, Medic, Nurse, etc.
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<RoleMenuPermission> MenuPermissions { get; set; } = new List<RoleMenuPermission>();
        public ICollection<RoleSubMenuPermission> SubMenuPermissions { get; set; } = new List<RoleSubMenuPermission>();
    }
}
