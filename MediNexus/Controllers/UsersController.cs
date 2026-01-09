using MediNexus.Api.Contracts.Users;
using MediNexus.Domain.Users;
using MediNexus.Infrastructure.Persistence;
using MediNexus.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly MediNexusDbContext _db;
        private readonly IPasswordHasher _hasher;

        public UsersController(MediNexusDbContext db, IPasswordHasher hasher)
        {
            _db = db; _hasher = hasher;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register([FromBody] CreateUserRequest req)
        {
            // ===== Validaciones de unicidad =====
            if (await _db.Users.AnyAsync(u => u.Email == req.Email))
                return Conflict("Email already exists.");

            if (await _db.Users.AnyAsync(u => u.Username == req.Username))
                return Conflict("Username already exists.");

            if (await _db.Users.AnyAsync(u => u.DocumentNumber == req.DocumentNumber))
                return Conflict("Document number already exists.");

            // ===== Validación de catálogos =====
            if (!await _db.DocumentTypes.AnyAsync(d => d.Id == req.DocumentTypeId && d.IsActive))
                return BadRequest("Invalid document type.");

            if (!await _db.UserProfiles.AnyAsync(p => p.Id == req.UserProfileId && p.IsActive))
                return BadRequest("Invalid user profile.");

            if (!await _db.UserRoles.AnyAsync(r => r.Id == req.UserRoleId && r.IsActive))
                return BadRequest("Invalid user role.");

            if (!await _db.UserStatuses.AnyAsync(s => s.Id == req.UserStatusId && s.IsActive))
                return BadRequest("Invalid user status.");

            // ===== Crear entidad User =====
            var user = new User
            {
                DocumentTypeId = req.DocumentTypeId,
                DocumentNumber = req.DocumentNumber.Trim(),
                FirstName = req.FirstName.Trim(),
                PasswordHash = _hasher.Hash(req.Password),
                LastName = req.LastName.Trim(),
                Username = req.Username.Trim(),
                Email = req.Email.Trim(),
                UserProfileId = req.UserProfileId,
                UserRoleId = req.UserRoleId,
                UserStatusId = req.UserStatusId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // TODO: ajustar según tu implementación real de password hashing
            // user.PasswordHash = _passwordHasher.HashPassword(user, req.Password);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // ===== Recargar con Include para traer nombres =====
            var created = await _db.Users
                .Include(u => u.DocumentType)
                .Include(u => u.UserProfile)
                .Include(u => u.UserRole)
                .Include(u => u.UserStatus)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            // ===== Respuesta completa =====
            var response = new UserResponse(
                created.Id,
                created.DocumentTypeId,
                created.DocumentType?.Name,
                created.DocumentNumber,
                created.FirstName,
                created.LastName,
                created.Username,
                created.Email,
                created.UserProfileId,
                created.UserProfile?.Name,
                created.UserRoleId,
                created.UserRole?.Name,
                created.UserStatusId,
                created.UserStatus?.Name,
                created.IsActive,
                created.CreatedAt
            );

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }


        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserResponse>> GetMe()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value
                        ?? User.Claims.First(c => c.Type == "email").Value;

            var user = await _db.Users
                .Include(u => u.DocumentType)
                .Include(u => u.UserProfile)
                .Include(u => u.UserRole)
                .Include(u => u.UserStatus)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user is null) return NotFound();

            return new UserResponse(
                user.Id,
                user.DocumentTypeId,
                user.DocumentType?.Name,   // 👈 Nombre del tipo de documento
                user.DocumentNumber,
                user.FirstName,
                user.LastName,
                user.Username,
                user.Email,
                user.UserProfileId,
                user.UserProfile?.Name,    // 👈 Nombre del perfil
                user.UserRoleId,
                user.UserRole?.Name,       // 👈 Nombre del rol
                user.UserStatusId,
                user.UserStatus?.Name,     // 👈 Nombre del estado
                user.IsActive,
                user.CreatedAt
            );
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAll()
        {
            var users = await _db.Users
                .Include(u => u.DocumentType)
                .Include(u => u.UserProfile)
                .Include(u => u.UserRole)
                .Include(u => u.UserStatus)
                .AsNoTracking()
                .ToListAsync();

            var result = users.Select(user => new UserResponse(
                user.Id,
                user.DocumentTypeId,
                user.DocumentType?.Name,
                user.DocumentNumber,
                user.FirstName,
                user.LastName,
                user.Username,
                user.Email,
                user.UserProfileId,
                user.UserProfile?.Name,
                user.UserRoleId,
                user.UserRole?.Name,
                user.UserStatusId,
                user.UserStatus?.Name,
                user.IsActive,
                user.CreatedAt
            ));

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserResponse>> GetById(int id)
        {
            var user = await _db.Users
                .Include(u => u.DocumentType)
                .Include(u => u.UserProfile)
                .Include(u => u.UserRole)
                .Include(u => u.UserStatus)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                return NotFound("User not found.");

            return new UserResponse(
                user.Id,
                user.DocumentTypeId,
                user.DocumentType?.Name,
                user.DocumentNumber,
                user.FirstName,
                user.LastName,
                user.Username,
                user.Email,
                user.UserProfileId,
                user.UserProfile?.Name,
                user.UserRoleId,
                user.UserRole?.Name,
                user.UserStatusId,
                user.UserStatus?.Name,
                user.IsActive,
                user.CreatedAt
            );
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<UserResponse>> Update(int id, [FromBody] UpdateUserRequest req)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
                return NotFound("User not found.");

            // Validaciones de unicidad (ignorando el mismo usuario)
            if (await _db.Users.AnyAsync(u => u.Id != id && u.Email == req.Email))
                return Conflict("Email already exists.");

            if (await _db.Users.AnyAsync(u => u.Id != id && u.Username == req.Username))
                return Conflict("Username already exists.");

            if (await _db.Users.AnyAsync(u => u.Id != id && u.DocumentNumber == req.DocumentNumber))
                return Conflict("Document number already exists.");

            // Validación de catálogos
            if (!await _db.DocumentTypes.AnyAsync(d => d.Id == req.DocumentTypeId && d.IsActive))
                return BadRequest("Invalid document type.");

            if (!await _db.UserProfiles.AnyAsync(p => p.Id == req.UserProfileId && p.IsActive))
                return BadRequest("Invalid user profile.");

            if (!await _db.UserRoles.AnyAsync(r => r.Id == req.UserRoleId && r.IsActive))
                return BadRequest("Invalid user role.");

            if (!await _db.UserStatuses.AnyAsync(s => s.Id == req.UserStatusId && s.IsActive))
                return BadRequest("Invalid user status.");

            // ===== ASIGNACIÓN DE CAMPOS =====
            user.DocumentTypeId = req.DocumentTypeId;
            user.DocumentNumber = req.DocumentNumber.Trim();
            user.FirstName = req.FirstName.Trim();
            user.LastName = req.LastName.Trim();
            user.Username = req.Username.Trim();
            user.Email = req.Email.Trim();
            user.UserProfileId = req.UserProfileId;
            user.UserRoleId = req.UserRoleId;
            user.UserStatusId = req.UserStatusId;
            user.IsActive = req.IsActive;

            await _db.SaveChangesAsync();

            // ===== RECARGAR CON INCLUDE PARA TRAER NOMBRES =====
            var updated = await _db.Users
                .Include(u => u.DocumentType)
                .Include(u => u.UserProfile)
                .Include(u => u.UserRole)
                .Include(u => u.UserStatus)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            // ===== RESPUESTA COMPLETA =====
            var response = new UserResponse(
                updated.Id,
                updated.DocumentTypeId,
                updated.DocumentType?.Name,
                updated.DocumentNumber,
                updated.FirstName,
                updated.LastName,
                updated.Username,
                updated.Email,
                updated.UserProfileId,
                updated.UserProfile?.Name,
                updated.UserRoleId,
                updated.UserRole?.Name,
                updated.UserStatusId,
                updated.UserStatus?.Name,
                updated.IsActive,
                updated.CreatedAt
            );

            return Ok(response);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
                return NotFound("User not found.");

            // Eliminación lógica
            user.IsActive = false;
            user.UserStatusId = 3; 

            await _db.SaveChangesAsync();

            return Ok("User was successfully deactivated.");
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("document-types")]
        public async Task<ActionResult<IEnumerable<object>>> GetDocumentTypes()
        {
            var docs = await _db.DocumentTypes
                .Where(d => d.IsActive)
                .OrderBy(d => d.Name)
                .Select(d => new
                {
                    d.Id,
                    d.Code,
                    d.Name,
                    d.IsActive
                })
                .ToListAsync();

            return Ok(docs);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("UserStatuses")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserStatuses()
        {
            var docs = await _db.UserStatuses
                .Where(d => d.IsActive)
                .OrderBy(d => d.Name)
                .Select(d => new
                {
                    d.Id,
                    d.Code,
                    d.Name,
                    d.IsActive
                })
                .ToListAsync();

            return Ok(docs);
        }

    }
}
