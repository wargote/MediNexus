using System;

namespace MediNexus.Domain.Tariffs
{
    public class TariffDetail
    {
        public int Id { get; set; }
        public int ReferenceCode { get; set; }
        public string Description { get; set; } = null!;
        public decimal Value { get; set; }
        public bool IsSurgicalProcedure { get; set; }
        public decimal Factors { get; set; }
        public int TariffId { get; set; }
        public int SurgicalGroupId { get; set; }

        public Tariff Tariff { get; set; } = null!;
        public SurgicalGroup SurgicalGroup { get; set; } = null!;
    }
}
