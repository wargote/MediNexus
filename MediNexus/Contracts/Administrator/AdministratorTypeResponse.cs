namespace MediNexus.Api.Contracts.Administrator
{
    public class AdministratorTypeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ShortName { get; set; }
    }
}
