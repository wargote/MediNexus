using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.Diagnoses
{
    public class Cie10CodeCreateRequest
    {
        [Required]
        [MaxLength(4)]
        public string Cod4 { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string DescripcionCodigoCuatroCaracteres { get; set; } = null!;
    }
}
