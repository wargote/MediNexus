namespace MediNexus.Domain.HCInicial
{
    public class AnalisisDiagnosticosPlanHCInicialDiagnostico
    {
        public int IdAnalisisDiagnosticosPlan { get; set; }
        public int IdCie10 { get; set; }

        public AnalisisDiagnosticosPlanHCInicial AnalisisDiagnosticosPlan { get; set; } = null!;
        public Cie10Code Cie10Code { get; set; } = null!;
    }
}
