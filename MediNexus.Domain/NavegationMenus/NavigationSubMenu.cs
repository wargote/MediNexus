using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediNexus.Domain.NavegationMenus
{
    public class NavigationSubMenu
    {
        public int Id { get; set; }

        public int MenuId { get; set; }
        public string Name { get; set; } = null!;
        public string Route { get; set; } = null!;
        public int SortOrder { get; set; } = 1;
        public bool IsActive { get; set; } = true;

        // Nav properties
        public NavigationMenu Menu { get; set; } = null!;
        public ICollection<RoleSubMenuPermission> RolePermissions { get; set; } = new List<RoleSubMenuPermission>();
    }
}
