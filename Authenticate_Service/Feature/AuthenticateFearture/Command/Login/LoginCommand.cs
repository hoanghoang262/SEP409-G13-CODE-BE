using Authenticate_Service.Common;
using Authenticate_Service.Models;
using Contract.Service.Message;
using Google;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authenticate_Service.Feature.AuthenticateFearture.Command.Login
{
    public class LoginCommand : IRequest<IActionResult>
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }

        public class LoginCommandHandler : IRequestHandler<LoginCommand, IActionResult>
        {
            private readonly IConfiguration _configuration;
            private readonly AuthenticationContext _context;
            private readonly HassPaword hash = new HassPaword();

            public LoginCommandHandler(IConfiguration configuration, AuthenticationContext context)
            {
                _configuration = configuration;
                _context = context;
            }

            public async Task<IActionResult> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    // Validate input
                    if (String.IsNullOrEmpty(request.UserName) || String.IsNullOrEmpty(request.Password))
                    {
                        return new BadRequestObjectResult(Message.MSG11);
                    }

                    var user = _context.Users.FirstOrDefault(u => u.UserName == request.UserName && u.Password == request.Password);
                    if (user != null)
                    {
                        // Check account status
                        if (user.Status == false)
                        {
                            return new BadRequestObjectResult(Message.MSG33);
                        }

                        // Check email confirmed
                        if (user.EmailConfirmed == false)
                        {
                            return new BadRequestObjectResult(Message.MSG03);
                        }

                        var userId = user.Id;
                        var userRoles = _context.Users.Include(c=>c.Role).FirstOrDefault(c=>c.UserName.Equals(request.UserName)).Role.Name;


                        var authClaims = new List<Claim>
                        {
                             new Claim("UserID", user.Id.ToString()),
                             new Claim("UserName", user.UserName)

                        };


                      

                       // var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));


                        var issuer = _configuration["Jwt:Issuer"];
                        var audience = _configuration["Jwt:Audience"];
                        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                        var signingCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha512Signature
                    );
                        var subject = new ClaimsIdentity(new[]
                         {
                             new Claim("UserID", user.Id.ToString()),
                             new Claim("UserName", user.UserName),
                             new Claim("Roles", userRoles),
                             new Claim(ClaimTypes.Role, userRoles)
                          });


                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = subject,
                            Expires = DateTime.UtcNow.AddMinutes(10),
                            Issuer = issuer,
                            Audience = audience,
                            SigningCredentials = signingCredentials
                        };
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var jwtToken = tokenHandler.WriteToken(token);


                        return new OkObjectResult(new
                        {
                            token = jwtToken,
                            expiration = token.ValidTo
                        });
                    }
                    else if (user == null)
                    {
                        return new BadRequestObjectResult(Message.MSG01);
                    }

                    return new OkObjectResult(Message.MSG04);
                }
                catch (GoogleApiException)
                {
                    return new StatusCodeResult(StatusCodes.Status503ServiceUnavailable);
                }
            }

            public bool CheckUserExist(string email)
            {
                var userExist = _context.Users.FirstOrDefault(x => x.Email.Equals(email));
                if (userExist == null)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
