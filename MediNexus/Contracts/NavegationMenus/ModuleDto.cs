namespace MediNexus.Api.Contracts.NavegationMenus
{
    public class ModuleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int SortOrder { get; set; }
        public List<MenuDto> Menus { get; set; } = new();
    }
}
