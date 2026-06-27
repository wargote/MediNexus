using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.HCInicial
{
    public class SubjetivoHCInicialCreateRequest
    {
        [Required(ErrorMessage = "El motivo de consulta es obligatorio.")]
        [MaxLength(300, ErrorMessage = "El motivo de consulta no puede superar 300 caracteres.")]
        public string MotivoConsulta { get; set; } = null!;

        [Required(ErrorMessage = "La descripción de enfermedad actual es obligatoria.")]
        [MaxLength(1000, ErrorMessage = "La enfermedad actual no puede superar 1000 caracteres.")]
        public string EnfermedadActual { get; set; } = null!;
    }
}
