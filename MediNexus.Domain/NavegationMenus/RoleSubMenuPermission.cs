using MediNexus.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediNexus.Domain.NavegationMenus
{
    public class RoleSubMenuPermission
    {
        public int Id { get; set; }

        public int RoleId { get; set; }          // FK a UserRoles
        public int SubMenuId { get; set; }       // FK a NavigationSubMenus
        public bool CanView { get; set; } = true;

        // Nav properties
        public UserRole Role { get; set; } = null!;
        public NavigationSubMenu SubMenu { get; set; } = null!;
    }
}
