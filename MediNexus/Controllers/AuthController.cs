using MediNexus.Api.Contracts.Auth;
using MediNexus.Infrastructure.Persistence;
using MediNexus.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediNexus.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly MediNexusDbContext _db;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtService _jwt;

        public AuthController(MediNexusDbContext db, IPasswordHasher hasher, IJwtService jwt)
        {
            _db = db;           
            _hasher = hasher;   
            _jwt = jwt;         
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest req)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email && u.IsActive);
            if (user is null || !_hasher.Verify(req.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials.");

            var (token, exp) = _jwt.CreateToken(user.Id, user.Email, user.Role);
            return new AuthResponse(token, exp);
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout() => NoContent();
    }
}
