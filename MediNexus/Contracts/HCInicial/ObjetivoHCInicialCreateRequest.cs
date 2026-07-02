using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.HCInicial
{
    public class ObjetivoHCInicialCreateRequest
    {
        // Examen Físico
        [MaxLength(1000)] public string? CabezaCuello { get; set; }
        [MaxLength(1000)] public string? Torax { get; set; }
        [MaxLength(1000)] public string? Abdomen { get; set; }
        [MaxLength(1000)] public string? Extremidades { get; set; }
        [MaxLength(1000)] public string? SistemaNervioso { get; set; }
        [MaxLength(1000)] public string? OrganosSentidos { get; set; }
        [MaxLength(1000)] public string? Genitourinario { get; set; }

        // Antecedentes Personales y Familiares
        [MaxLength(1000)] public string? Padres { get; set; }
        [MaxLength(1000)] public string? PersonalesMedicos { get; set; }
        [MaxLength(1000)] public string? OtrosFamiliares { get; set; }
        [MaxLength(1000)] public string? Alergicos { get; set; }
        [MaxLength(1000)] public string? Quirurgicos { get; set; }
        [MaxLength(1000)] public string? Toxicos { get; set; }
        [MaxLength(1000)] public string? Transfusiones { get; set; }
        [MaxLength(1000)] public string? Habitos { get; set; }
        [MaxLength(1000)] public string? Traumas { get; set; }
    }
}
