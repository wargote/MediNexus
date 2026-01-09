using MediNexus.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediNexus.Domain.NavegationMenus
{
    public class RoleMenuPermission
    {
        public int Id { get; set; }

        public int RoleId { get; set; }          // FK a UserRoles
        public int MenuId { get; set; }          // FK a NavigationMenus
        public bool CanView { get; set; } = true;

        // Nav properties
        public UserRole Role { get; set; } = null!;
        public NavigationMenu Menu { get; set; } = null!;
    }
}
