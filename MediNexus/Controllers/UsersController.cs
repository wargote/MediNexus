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
        public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterUserRequest req)
        {
            if (await _db.Users.AnyAsync(u => u.Email == req.Email))
                return Conflict("Email already exists.");

            var user = new User
            {
                Name = req.Name,
                Email = req.Email,
                PasswordHash = _hasher.Hash(req.Password),
                Role = string.IsNullOrWhiteSpace(req.Role) ? "Medic" : req.Role
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMe), new { }, new UserResponse(user.Id, user.Name, user.Email, user.Role, user.IsActive, user.CreatedAt));
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserResponse>> GetMe()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value
                        ?? User.Claims.First(c => c.Type == "email").Value;

            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
            if (user is null) return NotFound();

            return new UserResponse(user.Id, user.Name, user.Email, user.Role, user.IsActive, user.CreatedAt);
        }

    }
}
