using System;

namespace MediNexus.Api.Contracts.Patient
{
    public class PatientMiniResponse
    {
        public string PrimerNombre { get; set; } = null!;
        public string? SegundoNombre { get; set; }
        public string PrimerApellido { get; set; } = null!;
        public string? SegundoApellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Sexo { get; set; }
    }
}
