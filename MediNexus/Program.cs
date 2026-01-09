using MediNexus.Infrastructure.Persistence;
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
         .AllowAnyHeader()            // Authorization, Content-Type, etc.
         .AllowAnyMethod()            // GET, POST, PUT, DELETE, ...
         .AllowCredentials();         // solo si usas cookies; con JWT no es necesario
                                      // Si NO usas cookies, quita .AllowCredentials() y usa .SetIsOriginAllowed(_ => true) solo en dev
    });
});

// DbContext (usa la misma clave "Sql")
// DbContext (misma clave "Sql")
builder.Services.AddDbContext<MediNexusDbContext>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString("Sql"),
        npgsql =>
        {
            // Resiliencia ante fallos transitorios (red/timeouts)
            npgsql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);

            // Timeout de comandos (segundos)
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


// ===== Aplicar migraciones y crear Admin si no existe =====
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<MediNexusDbContext>();
//    await db.Database.MigrateAsync();

//    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

//    var adminEmail = app.Configuration["Admin:Email"];
//    var adminPassword = app.Configuration["Admin:Password"];

//    if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
//    {
//        // Ajusta los nombres de propiedades según tu entidad User
//        var exists = await db.Set<User>()
//            .FirstOrDefaultAsync(u => u.Email == adminEmail);

//        if (exists is null)
//        {
//            var admin = new User
//            {
//                // Ejemplos de campos comunes — ajusta a tu modelo
//                Email = adminEmail,
//                Name = "System Admin",
//                Role = "Admin",           // o UserRole.Admin si usas enum
//                IsActive = true,
//                CreatedAt = DateTime.UtcNow
//            };

//            admin.PasswordHash = hasher.Hash(adminPassword);
//            db.Add(admin);
//            await db.SaveChangesAsync();
//        }
//    }
//}
// ===== fin seed =====

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
