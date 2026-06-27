using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.HCInicial
{
    public class HCInicialCreateRequest
    {
        [Required(ErrorMessage = "El ID de la admisión es obligatorio.")]
        public int AdmissionId { get; set; }

        public int? IdSubjetivoHCInicial { get; set; }
        public int? IdObjetivoHCInicial { get; set; }
        public int? IdSignosVitalesHCInicial { get; set; }
        public int? IdAnalisisDiagnosticosPlanHCInicial { get; set; }
    }
}
