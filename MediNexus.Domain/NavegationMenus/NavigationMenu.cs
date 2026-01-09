using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediNexus.Domain.NavegationMenus
{
    public class NavigationMenu
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Route { get; set; }
        public string? Icon { get; set; }
        public int SortOrder { get; set; } = 1;
        public bool IsActive { get; set; } = true;
        public ICollection<NavigationSubMenu> SubMenus { get; set; } = new List<NavigationSubMenu>();
        public ICollection<RoleMenuPermission> RolePermissions { get; set; } = new List<RoleMenuPermission>();
        public int? ModuleId { get; set; }
        public NavigationModule? Module { get; set; }

    }
}
