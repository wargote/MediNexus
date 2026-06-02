namespace MediNexus.Api.Contracts.Medicines
{
    public class ElementTypeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    public class ElementUsageResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
