using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.Diagnoses
{
    public class Cie10CodeUpdateRequest
    {
        [Required]
        [MaxLength(500)]
        public string DescripcionCodigoCuatroCaracteres { get; set; } = null!;
    }
}
