using MediNexus.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediNexus.Domain.Providers
{
    public class Provider
    {
        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string Name { get; set; } = string.Empty;
        public int IdentificationTypeId { get; set; }

        [Required, MaxLength(50)]
        public string Nit { get; set; } = string.Empty;

        [MaxLength(5)]
        public string? VerificationDigit { get; set; }

        [MaxLength(300)]
        public string? Address { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }

        public int? CityId { get; set; }

        [MaxLength(250)]
        public string? LegalRepresentative { get; set; }

        [MaxLength(50)]
        public string? DocumentType { get; set; }

        [MaxLength(50)]
        public string? DocumentNumber { get; set; }

        public string? LegalRepresentativeSign { get; set; }

        [MaxLength(100)]
        public string? DianResolution { get; set; }

        [MaxLength(20)]
        public string? Prefix { get; set; }

        public int? FromNumber { get; set; }
        public int? ToNumber { get; set; }

        public DateTime? ResolutionFromDate { get; set; } 
        public DateTime? ResolutionToDate { get; set; }   

        public string? ResolutionText { get; set; }       

        [MaxLength(150)]
        public string? Email { get; set; }

        public string? Logo { get; set; }                 

        [MaxLength(100)]
        public string? EnableCode { get; set; }          

        [MaxLength(100)]
        public string? Regimen { get; set; }              

        [MaxLength(250)]
        public string? InvoiceIssuerName { get; set; }   

        public string? InvoiceIssuerSign { get; set; }  

        public bool ApplyTax { get; set; } = true;

    }
}
