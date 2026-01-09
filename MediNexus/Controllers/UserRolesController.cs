using MediNexus.Api.Contracts.Users;
using MediNexus.Domain.Users;
using MediNexus.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers
{
    [ApiController]
    [Route("api/user-roles")]
    [Authorize(Roles = "Admin")]
    public class UserRolesController : ControllerBase
    {
        private readonly MediNexusDbContext _db;

        public UserRolesController(MediNexusDbContext db)
        {
            _db = db;
        }

        // GET: api/user-roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRoleResponse>>> GetAll()
        {
            var roles = await _db.UserRoles
                .AsNoTracking()
                .ToListAsync();

            var result = roles.Select(r => new UserRoleResponse(
                r.Id,
                r.Name,
                r.Description,
                r.IsActive
            ));

            return Ok(result);
        }

        // GET: api/user-roles/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserRoleResponse>> GetById(int id)
        {
            var role = await _db.UserRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (role is null)
                return NotFound("Role not found.");

            return new UserRoleResponse(
                role.Id,
                role.Name,
                role.Description,
                role.IsActive
            );
        }

        // POST: api/user-roles
        [HttpPost]
        public async Task<ActionResult<UserRoleResponse>> Create([FromBody] CreateUserRoleRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Name))
                return BadRequest("Name is required.");

            var exists = await _db.UserRoles.AnyAsync(r => r.Name == req.Name);
            if (exists)
                return Conflict("A role with the same name already exists.");

            var role = new UserRole
            {
                Name = req.Name.Trim(),
                Description = string.IsNullOrWhiteSpace(req.Description) ? null : req.Description.Trim(),
                IsActive = true
            };

            _db.UserRoles.Add(role);
            await _db.SaveChangesAsync();

            var response = new UserRoleResponse(
                role.Id,
                role.Name,
                role.Description,
                role.IsActive
            );

            return CreatedAtAction(nameof(GetById), new { id = role.Id }, response);
        }

        // PUT: api/user-roles/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<UserRoleResponse>> Update(
            int id,
            [FromBody] UpdateUserRoleRequest req)
        {
            var role = await _db.UserRoles.FirstOrDefaultAsync(r => r.Id == id);
            if (role is null)
                return NotFound("Role not found.");

            if (string.IsNullOrWhiteSpace(req.Name))
                return BadRequest("Name is required.");

            var duplicate = await _db.UserRoles
                .AnyAsync(r => r.Id != id && r.Name == req.Name);
            if (duplicate)
                return Conflict("A role with the same name already exists.");

            role.Name = req.Name.Trim();
            role.Description = string.IsNullOrWhiteSpace(req.Description) ? null : req.Description.Trim();
            role.IsActive = req.IsActive;

            await _db.SaveChangesAsync();

            var response = new UserRoleResponse(
                role.Id,
                role.Name,
                role.Description,
                role.IsActive
            );

            return Ok(response);
        }

        // DELETE: api/user-roles/{id}
        // Soft delete: solo marcar IsActive = false
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _db.UserRoles.FirstOrDefaultAsync(r => r.Id == id);
            if (role is null)
                return NotFound("Role not found.");

            role.IsActive = false;
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
