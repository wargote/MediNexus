namespace MediNexus.Api.Contracts.HCInicial
{
    public class HCInicialUpdateRequest
    {
        public int? IdSubjetivoHCInicial { get; set; }
        public int? IdObjetivoHCInicial { get; set; }
        public int? IdSignosVitalesHCInicial { get; set; }
        public int? IdAnalisisDiagnosticosPlanHCInicial { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
