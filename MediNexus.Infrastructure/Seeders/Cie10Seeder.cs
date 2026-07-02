using ClosedXML.Excel;
using MediNexus.Domain.HCInicial;
using MediNexus.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MediNexus.Infrastructure.Seeders
{
    public class Cie10Seeder
    {
        private readonly MediNexusDbContext _db;
        private readonly IConfiguration _config;
        private readonly ILogger<Cie10Seeder> _logger;

        public Cie10Seeder(MediNexusDbContext db, IConfiguration config, ILogger<Cie10Seeder> logger)
        {
            _db = db;
            _config = config;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            var filePath = _config["Seeding:Cie10CsvPath"];
            if (string.IsNullOrWhiteSpace(filePath))
            {
                _logger.LogInformation("Cie10 seeding skipped: Seeding:Cie10CsvPath not configured.");
                return;
            }

            if (!File.Exists(filePath))
            {
                _logger.LogWarning("Cie10 seeding skipped: file not found at '{Path}'.", filePath);
                return;
            }

            if (await _db.Cie10Codes.AnyAsync())
            {
                _logger.LogInformation("Cie10 seeding skipped: table already contains data.");
                return;
            }

            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            var records = extension is ".xlsx" or ".xls"
                ? ReadFromExcel(filePath)
                : ReadFromCsv(filePath);

            var now = DateTime.UtcNow;
            var batch = new List<Cie10Code>(500);
            int total = 0;

            foreach (var (codigo, descripcion) in records)
            {
                batch.Add(new Cie10Code
                {
                    Codigo = codigo.Length > 10 ? codigo[..10] : codigo,
                    Descripcion = descripcion.Length > 500 ? descripcion[..500] : descripcion,
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                });

                if (batch.Count >= 500)
                {
                    await _db.Cie10Codes.AddRangeAsync(batch);
                    await _db.SaveChangesAsync();
                    total += batch.Count;
                    _logger.LogInformation("Cie10 seeding in progress: {Total} records inserted...", total);
                    batch.Clear();
                }
            }

            if (batch.Count > 0)
            {
                await _db.Cie10Codes.AddRangeAsync(batch);
                await _db.SaveChangesAsync();
                total += batch.Count;
            }

            _logger.LogInformation("Cie10 seeding completed: {Total} total records inserted.", total);
        }

        private static IEnumerable<(string Codigo, string Descripcion)> ReadFromExcel(string path)
        {
            using var workbook = new XLWorkbook(path);
            var sheet = workbook.Worksheets.First();

            // Skip row 1 (header), read from row 2 onward
            foreach (var row in sheet.RowsUsed().Skip(1))
            {
                var codigo = row.Cell(1).GetString().Trim();
                var descripcion = row.Cell(2).GetString().Trim();

                if (!string.IsNullOrWhiteSpace(codigo) && !string.IsNullOrWhiteSpace(descripcion))
                    yield return (codigo, descripcion);
            }
        }

        private IEnumerable<(string Codigo, string Descripcion)> ReadFromCsv(string path)
        {
            var delimiterStr = _config["Seeding:Cie10CsvDelimiter"] ?? ",";
            var delimiter = delimiterStr[0];

            foreach (var line in File.ReadLines(path).Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var fields = ParseCsvLine(line, delimiter);
                if (fields.Length < 2) continue;

                var codigo = fields[0].Trim().Trim('"');
                var descripcion = fields[1].Trim().Trim('"');

                if (!string.IsNullOrWhiteSpace(codigo) && !string.IsNullOrWhiteSpace(descripcion))
                    yield return (codigo, descripcion);
            }
        }

        private static string[] ParseCsvLine(string line, char delimiter)
        {
            var fields = new List<string>();
            var current = new System.Text.StringBuilder();
            bool inQuotes = false;

            foreach (char c in line)
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == delimiter && !inQuotes)
                {
                    fields.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }
            fields.Add(current.ToString());

            return fields.ToArray();
        }
    }
}
