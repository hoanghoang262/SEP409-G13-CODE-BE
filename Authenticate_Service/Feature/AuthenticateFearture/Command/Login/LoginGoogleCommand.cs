using Authenticate_Service.Common;
using Authenticate_Service.Models;
using FirebaseAdmin.Auth;
using Google;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authenticate_Service.Feature.AuthenticateFearture.Command.Login
{
    public class LoginGoogleCommand : IRequest<IActionResult>
    {
        public string Email { get; set; }
        public string? PhotoURL { get; set; }
        public string UserName { get; set; }


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
                    if (string.IsNullOrEmpty(request.Email))
                    {
                        return new BadRequestObjectResult("Email is required");
                    }

                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(request.Email));

                    if (user == null)
                    {
                        // Create a new user if not found in the database
                        var userLoginGoogle = new User
                        {
                            Email = request.Email,
                            UserName = request.UserName,
                            RoleId = 1, // Assign a default role ID or handle roles as needed
                            EmailConfirmed = true,
                            ProfilePict = request.PhotoURL,
                            Status=true
                        };
                        _context.Users.Add(userLoginGoogle);
                        await _context.SaveChangesAsync();
                        user = userLoginGoogle; // Assign the new user to 'user'
                    }

                    // Retrieve user roles
                    var userRoles = _context.Users.Include(c => c.Role).FirstOrDefault(c => c.Email.Equals(user.Email)).Role.Name;

                    var authClaims = new List<Claim>
                {
                new Claim("UserID", user.Id.ToString()),
                new Claim("UserName", user.UserName),
                new Claim("Roles", userRoles),
                new Claim("PhotoUrl",user.ProfilePict),
                new Claim(ClaimTypes.Role, userRoles)
                  };

                    var issuer = _configuration["Jwt:Issuer"];
                    var audience = _configuration["Jwt:Audience"];
                    var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                    var signingCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha512Signature
                    );
                    var subject = new ClaimsIdentity(authClaims);
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
                catch (GoogleApiException)
                {
                    return new StatusCodeResult(StatusCodes.Status503ServiceUnavailable);
                }
            }

        }
    }
}