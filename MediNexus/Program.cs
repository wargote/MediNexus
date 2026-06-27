using MediNexus.Infrastructure.Persistence;
using MediNexus.Infrastructure.Seeders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MediNexus.Infrastructure.Security;
using System.Text;
using MediNexus.Domain.Users;

var builder = WebApplication.CreateBuilder(args);

const string CorsPolicy = "FrontendsAllowed";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(CorsPolicy, p =>
    {
        p.WithOrigins(
             "http://localhost:3000",
             "https://my-system-medic.vercel.app"
          )
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials();
    });
});

// DbContext
builder.Services.AddDbContext<MediNexusDbContext>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString("Sql"),
        npgsql =>
        {
            npgsql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            npgsql.CommandTimeout(60);
        }
    )
);

// Auth (JWT)
var jwt = builder.Configuration.GetSection("Jwt");
var keyBytes = Encoding.UTF8.GetBytes(jwt["Key"]!);

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ===== Aplicar migraciones pendientes al inicio =====
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MediNexusDbContext>();
    await db.Database.MigrateAsync();
}
// ===== fin migraciones =====

// ===== Seed CIE-10 =====
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MediNexusDbContext>();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
    var cie10Seeder = new Cie10Seeder(db, config, loggerFactory.CreateLogger<Cie10Seeder>());
    await cie10Seeder.SeedAsync();
}
// ===== fin seed CIE-10 =====

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
