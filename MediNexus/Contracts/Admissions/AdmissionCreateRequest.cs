using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.Admissions
{
    public class AdmissionCreateRequest
    {
        [Required(ErrorMessage = "El documento del paciente es obligatorio.")]
        public string Document { get; set; } = null!;

        [Required(ErrorMessage = "El ID de la modalidad de atención es obligatorio.")]
        public int CareModalityId { get; set; }

        [Required(ErrorMessage = "El ID del motivo de atención es obligatorio.")]
        public int CareReasonId { get; set; }

        [Required(ErrorMessage = "El ID de la clasificación de servicio es obligatoria.")]
        public int ServiceClassificationId { get; set; }

        [Required(ErrorMessage = "El ID del grupo de servicio es obligatorio.")]
        public int ServiceGroupId { get; set; }

        [Required(ErrorMessage = "El ID del tipo de ingreso es obligatorio.")]
        public int AdmissionTypeId { get; set; }

        [Required(ErrorMessage = "El ID del ámbito de atención es obligatorio.")]
        public int CareScopeId { get; set; }

        [Required(ErrorMessage = "El ID de la finalidad de atención es obligatoria.")]
        public int CarePurposeId { get; set; }

        [Required(ErrorMessage = "El ID de la EPS es obligatorio.")]
        public int EpsId { get; set; }

        [Required(ErrorMessage = "El ID del convenio es obligatorio.")]
        public int ConvenioId { get; set; }
    }
}
