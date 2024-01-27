    using Account.API.Model;
using Authenticate_Service.Models;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authenticate_Service.Feature.AuthenticateFearture.LoginGoogle;
using Authenticate_Service.Feature.AuthenticateFearture.Command;


namespace Authenticated.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly AuthenticationContext context;
        //private readonly GenerateJwtToken generateJwtToken =new GenerateJwtToken() ;
        private readonly MediatR.IMediator _mediator;


        public AuthenticateController(MediatR.IMediator mediator, IConfiguration configuration, AuthenticationContext _context)
        {

            _configuration = configuration;
            context = _context;
            _mediator = mediator;


        }
        [HttpPost]
        public async Task<IActionResult> LoginGoogle(LoginGoogleCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModels model)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == model.Email
                                                && u.Password == model.Password);


            if (user != null)
            {
                var userRoles = (from u in context.Users
                                 where user.Email == model.Email
                                 join role in context.Roles on u.RoleId equals role.Id
                                 select role.Name).ToList();

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),

                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }


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

    }
 }
