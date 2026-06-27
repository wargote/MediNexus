using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.HCInicial
{
    public class EvolucionEspecialistaCreateRequest
    {
        [Required(ErrorMessage = "El ID de admisión es obligatorio.")]
        public int AdmissionId { get; set; }

        [Required(ErrorMessage = "El motivo de consulta es obligatorio.")]
        [MaxLength(1000, ErrorMessage = "El motivo de consulta no puede superar 1000 caracteres.")]
        public string MotivoConsulta { get; set; } = null!;

        [Required(ErrorMessage = "El plan del especialista es obligatorio.")]
        [MaxLength(1000, ErrorMessage = "El plan no puede superar 1000 caracteres.")]
        public string Plan { get; set; } = null!;
    }
}
