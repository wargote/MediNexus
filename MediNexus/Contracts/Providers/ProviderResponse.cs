using System.ComponentModel.DataAnnotations;

namespace MediNexus.Api.Contracts.Providers
{
    public record ProviderResponse(
        int Id,
        string Name,
        int IdentificationTypeId,
        string Nit,
        string? VerificationDigit,
        string? Address,
        string? Phone,
        int? CityId,
        string? LegalRepresentative,
        string? DocumentType,
        string? DocumentNumber,
        string? LegalRepresentativeSign,
        string? DianResolution,
        string? Prefix,
        int? FromNumber,
        int? ToNumber,
        DateTime? ResolutionFromDate,
        DateTime? ResolutionToDate,
        string? ResolutionText,
        string? Email,
        string? Logo,
        string? EnableCode,
        string? Regimen,
        string? InvoiceIssuerName,
        string? InvoiceIssuerSign,
        bool ApplyTax
    );

    public class CreateProviderRequest
    {
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

        [EmailAddress, MaxLength(150)]
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

    public class UpdateProviderRequest : CreateProviderRequest
    {
        // Mismo shape; heredar ayuda a no duplicar.
    }
}
