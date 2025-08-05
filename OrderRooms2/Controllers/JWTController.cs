using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.IdentityModel.Tokens;
using OrderRooms2.Managers;
using OrderRooms2.ModelDto;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Threading.Tasks;


namespace TestJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTController : ControllerBase
    {
        readonly ManagerJWT _manJWT;
        public JWTController(ManagerJWT jwt)
        {
            _manJWT = jwt;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> GivMEToken([FromBody] User user)
        {
            return await _manJWT.GivMEToken(user);
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> RegistrationAsync([FromBody] User user)
        {
            return await _manJWT.RegistrationAsync(user);
        }
        [Authorize]
        [HttpGet("GetAllUsers")]
        public async Task<IEnumerable<IdentityUser>> GetAllClients()
        {
            return await _manJWT.GetAllClients();
        }
    }

}

