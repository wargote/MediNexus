using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.Admissions
{
    public class AdmissionUpdateRequest
    {
        [Required(ErrorMessage = "La modalidad de atención es obligatoria.")]
        public string ModalidadAtencion { get; set; } = null!;

        [Required(ErrorMessage = "El motivo de atención es obligatorio.")]
        public string MotivoAtencion { get; set; } = null!;

        [Required(ErrorMessage = "La clasificación de servicio es obligatoria.")]
        public string ClasificacionServicio { get; set; } = null!;

        [Required(ErrorMessage = "El grupo de servicio es obligatorio.")]
        public string GrupoServicio { get; set; } = null!;

        [Required(ErrorMessage = "El tipo de ingreso es obligatorio.")]
        public string Ingreso { get; set; } = null!;

        [Required(ErrorMessage = "El ámbito de atención es obligatorio.")]
        public string AmbitoAtencion { get; set; } = null!;

        [Required(ErrorMessage = "La finalidad de atención es obligatoria.")]
        public string FinalidadAtencion { get; set; } = null!;

        [Required(ErrorMessage = "El ID de la EPS es obligatorio.")]
        public int EpsId { get; set; }

        [Required(ErrorMessage = "El ID del convenio es obligatorio.")]
        public int ConvenioId { get; set; }
    }
}
