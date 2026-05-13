using System;

namespace MediNexus.Api.Contracts.TariffDetail
{
    public class TariffDetailResponse
    {
        public int Id { get; set; }
        public int ReferenceCode { get; set; }
        public string Description { get; set; } = null!;
        public decimal Value { get; set; }
        public bool IsSurgicalProcedure { get; set; }
        public decimal Factors { get; set; }
        public int TariffId { get; set; }
        public string? TariffName { get; set; }
        public int SurgicalGroupId { get; set; }
        public string? SurgicalGroupReferenceCode { get; set; }
    }
}
