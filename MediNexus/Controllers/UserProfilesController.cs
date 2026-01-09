using MediNexus.Api.Contracts.Users;
using MediNexus.Domain.Users;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers
{
    [ApiController]
    [Route("api/user-profiles")]
    [Authorize(Roles = "Admin")]
    public class UserProfilesController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public UserProfilesController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/user-profiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfileResponse>>> GetAll()
        {
            var profiles = await _db.UserProfiles
                .Include(p => p.UserRole)
                .AsNoTracking()
                .ToListAsync();

            var result = profiles.Select(p => new UserProfileResponse(
                p.Id,
                p.Name,
                p.Description,
                p.UserRoleId,
                p.UserRole?.Name,
                p.IsActive
            ));

            return Ok(result);
        }

        // GET: api/user-profiles/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserProfileResponse>> GetById(int id)
        {
            var profile = await _db.UserProfiles
                .Include(p => p.UserRole)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (profile is null)
                return NotFound("User profile not found.");

            return new UserProfileResponse(
                profile.Id,
                profile.Name,
                profile.Description,
                profile.UserRoleId,
                profile.UserRole?.Name,
                profile.IsActive
            );
        }

        // POST: api/user-profiles
        [HttpPost]
        public async Task<ActionResult<UserProfileResponse>> Create([FromBody] CreateUserProfileRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Name))
                return BadRequest("Name is required.");

            // Validar nombre repetido
            var exists = await _db.UserProfiles.AnyAsync(p => p.Name == req.Name);
            if (exists)
                return Conflict("A profile with the same name already exists.");

            // Validar que el rol exista
            var roleExists = await _db.UserRoles.AnyAsync(r => r.Id == req.UserRoleId);
            if (!roleExists)
                return BadRequest("UserRoleId does not exist.");

            var profile = new UserProfile
            {
                Name = req.Name.Trim(),
                Description = string.IsNullOrWhiteSpace(req.Description) ? null : req.Description.Trim(),
                UserRoleId = req.UserRoleId,
                IsActive = true
            };

            _db.UserProfiles.Add(profile);
            await _db.SaveChangesAsync();

            // Recargar con Include para obtener el nombre del rol
            var created = await _db.UserProfiles
                .Include(p => p.UserRole)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == profile.Id);

            var response = new UserProfileResponse(
                created.Id,
                created.Name,
                created.Description,
                created.UserRoleId,
                created.UserRole?.Name,
                created.IsActive
            );

            return CreatedAtAction(nameof(GetById), new { id = profile.Id }, response);
        }


        // PUT: api/user-profiles/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<UserProfileResponse>> Update(int id,[FromBody] UpdateUserProfileRequest req)
        {
            var profile = await _db.UserProfiles.FirstOrDefaultAsync(p => p.Id == id);
            if (profile is null)
                return NotFound("User profile not found.");

            if (string.IsNullOrWhiteSpace(req.Name))
                return BadRequest("Name is required.");

            // Validar nombre duplicado
            var duplicate = await _db.UserProfiles
                .AnyAsync(p => p.Id != id && p.Name == req.Name);
            if (duplicate)
                return Conflict("A profile with the same name already exists.");

            // Validar que el rol exista
            var roleExists = await _db.UserRoles.AnyAsync(r => r.Id == req.UserRoleId);
            if (!roleExists)
                return BadRequest("UserRoleId does not exist.");

            // Actualizar valores
            profile.Name = req.Name.Trim();
            profile.Description = string.IsNullOrWhiteSpace(req.Description) ? null : req.Description.Trim();
            profile.IsActive = req.IsActive;
            profile.UserRoleId = req.UserRoleId;

            await _db.SaveChangesAsync();

            // Recargar con Include para obtener el nombre del rol actualizado
            var updated = await _db.UserProfiles
                .Include(p => p.UserRole)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            var response = new UserProfileResponse(
                updated.Id,
                updated.Name,
                updated.Description,
                updated.UserRoleId,
                updated.UserRole?.Name,
                updated.IsActive
            );

            return Ok(response);
        }


        // DELETE: api/user-profiles/{id}
        // Soft delete marcando IsActive = false
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var profile = await _db.UserProfiles.FirstOrDefaultAsync(p => p.Id == id);
            if (profile is null)
                return NotFound("User profile not found.");

            profile.IsActive = false;
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
