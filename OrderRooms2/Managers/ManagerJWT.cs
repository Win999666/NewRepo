using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrderRooms2.ModelDto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OrderRooms2.Managers
{
    public class ManagerJWT
    {
        UserManager<IdentityUser> _manager;
        RoleManager<IdentityRole> _roleManager;
        public ManagerJWT(UserManager<IdentityUser> usermanager, RoleManager<IdentityRole> role)
        {
            _manager = usermanager;
            _roleManager = role;
        }
        
        public async Task<IActionResult> GivMEToken( User user)
        {
            IdentityUser? user1 = await _manager.FindByEmailAsync(user.Mail);
            if (user1 == null)
            {
                return new BadRequestObjectResult("not find");
            }
            if (!await _manager.CheckPasswordAsync(user1, user.Pasword))
            {
                return new UnauthorizedObjectResult(null);
            }
            JwtSecurityToken token = await Login(user, user1);

            return new OkObjectResult(new JwtSecurityTokenHandler().WriteToken(token));

        }

        private async Task<JwtSecurityToken> Login(User user, IdentityUser user1)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Mail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };
            foreach (var role in await _manager.GetRolesAsync(user1))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            string key = "12345123451234512345123451234512";
            SymmetricSecurityKey summetricKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("12345123451234512345123451234512"));

            var perec = new SigningCredentials(summetricKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: null,
                signingCredentials: perec
                );
            return token;
        }
        
        public async Task<IActionResult> RegistrationAsync( User user)
        {
            IdentityUser idenUser = new IdentityUser { UserName = user.Mail, Email = user.Mail };
            var result = await _manager.CreateAsync(idenUser, user.Pasword);
            if (result.Succeeded == false)
            {
                return new BadRequestObjectResult(result.Errors);
            }
            if (user.Mail == "gal999@gmail.com")
            {
                var check = await _manager.AddToRoleAsync(idenUser, "Admin");
                if (!check.Succeeded)
                    return new BadRequestObjectResult(check.Errors);
            }
            JwtSecurityToken token = await Login(user, idenUser);

            return new OkObjectResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
        
        
        public async Task<IEnumerable<IdentityUser>> GetAllClients()
        {
            return _manager.Users.ToList();
        }

    }
}

