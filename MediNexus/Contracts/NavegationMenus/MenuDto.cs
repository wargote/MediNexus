namespace MediNexus.Api.Contracts.NavegationMenus
{
    public class MenuDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Route { get; set; }
        public string? Icon { get; set; }
        public int SortOrder { get; set; }
        public List<SubMenuDto> SubMenus { get; set; } = new();
    }
}
