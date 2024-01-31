using Authenticate_Service.Common;
using Authenticate_Service.Models;
using FirebaseAdmin.Auth;
using Google.Apis.Auth;
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
                    string googleIdToken = request.IdToken;

                    GoogleJsonWebSignature.ValidationSettings validationSettings = new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { "597795403161-d1i3etbsgv2stk0ktoge8mi3amt0euiu.apps.googleusercontent.com" } // Replace with your Google API client ID
                    };

                    var payload = await GoogleJsonWebSignature.ValidateAsync(googleIdToken, validationSettings);

                    // The token is valid. You can access user information from the payload.
                    string userId = payload.Subject;
                    string userEmail = payload.Email;
                    string userName = payload.GivenName;

                    var checkUser = CheckUserExist(userEmail);

                    if (checkUser == false)
                    {
                        var userLoginGoogle = new User
                        {
                            Email = userEmail
                        };
                        _context.Users.Add(userLoginGoogle);
                        await _context.SaveChangesAsync();
                    }
                    var getUserId= _context.Users.FirstOrDefault(x => x.Email.Equals(userEmail)).Id;

                    var tokenGenerator = new GenerateJwtToken(_configuration);

                    var token = tokenGenerator.GenerateToken(getUserId,userEmail, new List<string> { "Student" });

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

            public bool CheckUserExist(string email)
            {
                var userExist = _context.Users.FirstOrDefault(x => x.Email.Equals(email));
                if(userExist == null)
                {
                    return false;
                }
                return true;

            }
        }
    } 
    
}