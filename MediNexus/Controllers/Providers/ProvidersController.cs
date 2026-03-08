using MediNexus.Api.Contracts.Providers;
using MediNexus.Domain.Providers;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace MediNexus.Api.Controllers.Providers
{
    [ApiController]
    [Route("api/providers")]
    public class ProvidersController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public ProvidersController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/providers?search=xxx&cityId=1&page=1&pageSize=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProviderResponse>>> GetAll(
            [FromQuery] string? search,
            [FromQuery] int? cityId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 50 : Math.Min(pageSize, 200);

            IQueryable<Provider> query = _db.Providers.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(x =>
                    x.Name.Contains(s) ||
                    x.Nit.Contains(s) ||
                    (x.Email != null && x.Email.Contains(s)));
            }

            if (cityId.HasValue)
                query = query.Where(x => x.CityId == cityId.Value);

            var items = await query
                .OrderBy(x => x.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ProviderResponse(
                    x.Id, x.Name, x.IdentificationTypeId, x.Nit, x.VerificationDigit,
                    x.Address, x.Phone, x.CityId, x.LegalRepresentative, x.DocumentType,
                    x.DocumentNumber, x.LegalRepresentativeSign, x.DianResolution, x.Prefix,
                    x.FromNumber, x.ToNumber, x.ResolutionFromDate, x.ResolutionToDate,
                    x.ResolutionText, x.Email, x.Logo, x.EnableCode, x.Regimen,
                    x.InvoiceIssuerName, x.InvoiceIssuerSign, x.ApplyTax
                ))
                .ToListAsync();

            return Ok(items);
        }

        // GET: api/providers/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProviderResponse>> GetById([FromRoute] int id)
        {
            var item = await _db.Providers
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new ProviderResponse(
                    x.Id, x.Name, x.IdentificationTypeId, x.Nit, x.VerificationDigit,
                    x.Address, x.Phone, x.CityId, x.LegalRepresentative, x.DocumentType,
                    x.DocumentNumber, x.LegalRepresentativeSign, x.DianResolution, x.Prefix,
                    x.FromNumber, x.ToNumber, x.ResolutionFromDate, x.ResolutionToDate,
                    x.ResolutionText, x.Email, x.Logo, x.EnableCode, x.Regimen,
                    x.InvoiceIssuerName, x.InvoiceIssuerSign, x.ApplyTax
                ))
                .FirstOrDefaultAsync();

            if (item is null) return NotFound("Provider not found.");
            return Ok(item);
        }

        // POST: api/providers
        [HttpPost]
        public async Task<ActionResult<ProviderResponse>> Create([FromBody] CreateProviderRequest request)
        {
            var nit = request.Nit.Trim();
            var name = request.Name.Trim();

            var exists = await _db.Providers.AnyAsync(x => x.Nit == nit);
            if (exists) return Conflict("Provider Nit already exists.");

            var entity = new Provider
            {
                Name = name,
                IdentificationTypeId = request.IdentificationTypeId,
                Nit = nit,
                VerificationDigit = request.VerificationDigit?.Trim(),
                Address = request.Address?.Trim(),
                Phone = request.Phone?.Trim(),
                CityId = request.CityId,
                LegalRepresentative = request.LegalRepresentative?.Trim(),
                DocumentType = request.DocumentType?.Trim(),
                DocumentNumber = request.DocumentNumber?.Trim(),
                LegalRepresentativeSign = request.LegalRepresentativeSign,
                DianResolution = request.DianResolution?.Trim(),
                Prefix = request.Prefix?.Trim(),
                FromNumber = request.FromNumber,
                ToNumber = request.ToNumber,
                ResolutionFromDate = request.ResolutionFromDate?.Date,
                ResolutionToDate = request.ResolutionToDate?.Date,
                ResolutionText = request.ResolutionText,
                Email = request.Email?.Trim(),
                Logo = request.Logo,
                EnableCode = request.EnableCode?.Trim(),
                Regimen = request.Regimen?.Trim(),
                InvoiceIssuerName = request.InvoiceIssuerName?.Trim(),
                InvoiceIssuerSign = request.InvoiceIssuerSign,
                ApplyTax = request.ApplyTax
            };

            _db.Providers.Add(entity);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict("Provider Nit already exists.");
            }

            return CreatedAtAction(nameof(GetById), new { id = entity.Id },
                new ProviderResponse(
                    entity.Id, entity.Name, entity.IdentificationTypeId, entity.Nit, entity.VerificationDigit,
                    entity.Address, entity.Phone, entity.CityId, entity.LegalRepresentative, entity.DocumentType,
                    entity.DocumentNumber, entity.LegalRepresentativeSign, entity.DianResolution, entity.Prefix,
                    entity.FromNumber, entity.ToNumber, entity.ResolutionFromDate, entity.ResolutionToDate,
                    entity.ResolutionText, entity.Email, entity.Logo, entity.EnableCode, entity.Regimen,
                    entity.InvoiceIssuerName, entity.InvoiceIssuerSign, entity.ApplyTax
                ));
        }

        // PUT: api/providers/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProviderResponse>> Update([FromRoute] int id, [FromBody] UpdateProviderRequest request)
        {
            var entity = await _db.Providers.FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null) return NotFound("Provider not found.");

            var nit = request.Nit.Trim();

            var exists = await _db.Providers.AnyAsync(x => x.Id != id && x.Nit == nit);
            if (exists) return Conflict("Provider Nit already exists.");

            entity.Name = request.Name.Trim();
            entity.IdentificationTypeId = request.IdentificationTypeId;
            entity.Nit = nit;
            entity.VerificationDigit = request.VerificationDigit?.Trim();
            entity.Address = request.Address?.Trim();
            entity.Phone = request.Phone?.Trim();
            entity.CityId = request.CityId;
            entity.LegalRepresentative = request.LegalRepresentative?.Trim();
            entity.DocumentType = request.DocumentType?.Trim();
            entity.DocumentNumber = request.DocumentNumber?.Trim();
            entity.LegalRepresentativeSign = request.LegalRepresentativeSign;
            entity.DianResolution = request.DianResolution?.Trim();
            entity.Prefix = request.Prefix?.Trim();
            entity.FromNumber = request.FromNumber;
            entity.ToNumber = request.ToNumber;
            entity.ResolutionFromDate = request.ResolutionFromDate?.Date;
            entity.ResolutionToDate = request.ResolutionToDate?.Date;
            entity.ResolutionText = request.ResolutionText;
            entity.Email = request.Email?.Trim();
            entity.Logo = request.Logo;
            entity.EnableCode = request.EnableCode?.Trim();
            entity.Regimen = request.Regimen?.Trim();
            entity.InvoiceIssuerName = request.InvoiceIssuerName?.Trim();
            entity.InvoiceIssuerSign = request.InvoiceIssuerSign;
            entity.ApplyTax = request.ApplyTax;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict("Provider Nit already exists.");
            }

            return Ok(new ProviderResponse(
                entity.Id, entity.Name, entity.IdentificationTypeId, entity.Nit, entity.VerificationDigit,
                entity.Address, entity.Phone, entity.CityId, entity.LegalRepresentative, entity.DocumentType,
                entity.DocumentNumber, entity.LegalRepresentativeSign, entity.DianResolution, entity.Prefix,
                entity.FromNumber, entity.ToNumber, entity.ResolutionFromDate, entity.ResolutionToDate,
                entity.ResolutionText, entity.Email, entity.Logo, entity.EnableCode, entity.Regimen,
                entity.InvoiceIssuerName, entity.InvoiceIssuerSign, entity.ApplyTax
            ));
        }

        // DELETE: api/providers/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var entity = await _db.Providers.FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null) return NotFound("Provider not found.");

            _db.Providers.Remove(entity);

            // Nota: cuando Contracts tenga FK a Providers, esto puede fallar si está referenciado.
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
