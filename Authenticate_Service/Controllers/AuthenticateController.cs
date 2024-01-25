using Account.API;
using Account.API.Model;
using Authenticate_Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authenticated.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly AuthenticationContext context;


        public AuthenticateController(IConfiguration configuration,AuthenticationContext _context)
        {

            _configuration = configuration;
            context = _context;
        }
       
   
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModels model)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == model.Username
                                                && u.Password == model.Password);


            if (user != null)
            {
                var userRoles = user.Role;

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),

                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };



                //foreach (var userRole in userRoles)
                //{

                //}
               // authClaims.Add(new Claim("Role", userRoles));


                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpGet]

        public IActionResult getUser()
        {
            var user = context.Users.ToList();
            return Ok(user);
        }

    }

}
