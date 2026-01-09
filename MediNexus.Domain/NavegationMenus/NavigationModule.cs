using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediNexus.Domain.NavegationMenus
{
    public class NavigationModule
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int SortOrder { get; set; } = 1;
        public bool IsActive { get; set; } = true;

        public ICollection<NavigationMenu> Menus { get; set; } = new List<NavigationMenu>();
    }

}
