namespace MediNexus.Domain.Diagnoses
{
    public class Cie10Code
    {
        /// <summary>
        /// Código CIE-10 de cuatro caracteres (PK natural).
        /// </summary>
        public string Cod4 { get; set; } = null!;

        /// <summary>
        /// Descripción asociada al código CIE-10 de cuatro caracteres.
        /// </summary>
        public string DescripcionCodigoCuatroCaracteres { get; set; } = null!;
    }
}
