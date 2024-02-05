using Authenticate_Service.Common;
using Authenticate_Service.Models;
using FirebaseAdmin.Auth;
using Google;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authenticate_Service.Feature.AuthenticateFearture.Command.Login
{
    public class LoginGoogleCommand : IRequest<IActionResult>
    {
        public string Email { get; set; }
        


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
                    //string googleIdToken = request.IdToken;

                    //GoogleJsonWebSignature.ValidationSettings validationSettings = new GoogleJsonWebSignature.ValidationSettings
                    //{
                    //    Audience = new[] { "1023076690970-a2q73p8f7ub5ddrdjt3td5udre4j3e7l.apps.googleusercontent.com" }

                    //};

                    //var payload = await GoogleJsonWebSignature.ValidateAsync(googleIdToken, validationSettings);

                    //The token is valid.You can access user information from the payload.
                    //string userId = payload.Subject;
                    //string userEmail = payload.Email;
                    //string userName = payload.GivenName;

                    if (String.IsNullOrEmpty(request.Email))
                    {
                        return new  BadRequestObjectResult("Not found email");

                    }
                    var user = _context.Users.FirstOrDefault(x => x.Email.Equals(request.Email));

                    if (user != null && user.Password !=null)
                    {
                        return new  BadRequestObjectResult("A user is already registered with this e-mail address.");
                        
                    }
                    if(user==null)
                    {
                        var userLoginGoogle = new User
                        {
                            Email = request.Email,
                            UserName = request.Email.Split('@')[0],                         
                            RoleId = 1
                        };
                        _context.Users.Add(userLoginGoogle);
                        await _context.SaveChangesAsync();
                    }
                    var getUserId = _context.Users.FirstOrDefault(x => x.Email.Equals(request.Email)).Id;

                    var tokenGenerator = new GenerateJwtToken(_configuration);
                    var roles = new List<string> { "Student" };

                    var token = tokenGenerator.GenerateToken(getUserId, request.Email, roles);

                    return new OkObjectResult(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
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