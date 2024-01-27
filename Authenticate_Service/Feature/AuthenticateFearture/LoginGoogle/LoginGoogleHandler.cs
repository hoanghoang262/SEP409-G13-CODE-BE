using Authenticate_Service.Models;
using FirebaseAdmin.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authenticate_Service.Feature.AuthenticateFearture.LoginGoogle
{
    public class LoginGoogleCommand : IRequest<IActionResult>
    {
        public string? IdToken { get; set; }

        public class LoginGoogleCommandHandler : IRequestHandler<LoginGoogleCommand, IActionResult>
        {
            private readonly IConfiguration _configuration;
            private readonly AuthenticationContext _context;

            public LoginGoogleCommandHandler(IConfiguration configuration, AuthenticationContext context)
            {
                _configuration = configuration;
                _context = context;
            }

            public async Task<IActionResult> Handle(LoginGoogleCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(request.IdToken);

                    var email = decodedToken.Claims["email"].ToString();
                    var userExist = _context.Users.FirstOrDefault(x => x.Email.Equals(email));

                    if (userExist == null)
                    {
                        var userLoginGoogle = new User
                        {
                            Email = email
                        };
                        _context.Users.Add(userLoginGoogle);
                        await _context.SaveChangesAsync();
                    }

                    var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim("Role", "Student"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                    return new OkObjectResult(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });

                }
                catch (FirebaseAuthException)
                {
                    return new UnauthorizedResult();
                }

            }
        }
    }
}