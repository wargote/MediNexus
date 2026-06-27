using System;

namespace MediNexus.Api.Contracts.HCInicial
{
    public class ObjetivoHCInicialResponse
    {
        public int Id { get; set; }

        // Examen Físico
        public string? CabezaCuello { get; set; }
        public string? Torax { get; set; }
        public string? Abdomen { get; set; }
        public string? Extremidades { get; set; }
        public string? SistemaNervioso { get; set; }
        public string? OrganosSentidos { get; set; }
        public string? Genitourinario { get; set; }

        // Antecedentes Personales y Familiares
        public string? Padres { get; set; }
        public string? PersonalesMedicos { get; set; }
        public string? OtrosFamiliares { get; set; }
        public string? Alergicos { get; set; }
        public string? Quirurgicos { get; set; }
        public string? Toxicos { get; set; }
        public string? Transfusiones { get; set; }
        public string? Habitos { get; set; }
        public string? Traumas { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
