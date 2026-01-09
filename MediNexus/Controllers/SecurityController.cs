using MediNexus.Api.Contracts.Security;
using MediNexus.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;

namespace MediNexus.Api.Controllers
{
    [ApiController]
    [Route("api/security")]
    public class SecurityController : ControllerBase
    {
        private readonly IPasswordHasher _hasher;

        public SecurityController(IPasswordHasher hasher)
        {
            _hasher = hasher;
        }

        [HttpPost("hash")]
        public ActionResult<HashResponse> GenerateHash([FromBody] HashRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Password is required.");

            var hash = _hasher.Hash(req.Password);

            return Ok(new HashResponse(hash));
        }
    }
}
