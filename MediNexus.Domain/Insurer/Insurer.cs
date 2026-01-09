using MediNexus.Domain.Administrator;
using MediNexus.Domain.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediNexus.Domain.Insurer
{
    public class Insurer
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;      // Nombre_Aseguradora
        public string Nit { get; set; } = null!;       // niteps
        public short? VerificationDigit { get; set; }   // digito_verificacion
        public string? Code { get; set; }              // CodigoAseguradora
        public string? Address { get; set; }
        public int CityId { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }
        public int AdministratorTypeId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public City City { get; set; } = null!;
        public AdministratorType AdministratorType { get; set; } = null!;
    }
}
