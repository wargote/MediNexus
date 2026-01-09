using MediNexus.Domain.NavegationMenus;

namespace MediNexus.Api.Contracts.NavegationMenus
{
    public class NavigationResponse
    {
        public List<ModuleDto> Modules { get; set; } = new();
    }
}
