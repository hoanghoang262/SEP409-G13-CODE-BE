using Authenticate_Service.Common;
using Authenticate_Service.Models;
using Google;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Authenticate_Service.Feature.AuthenticateFearture.Command.Login
{
    public class LoginCommand : IRequest<IActionResult>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public class LoginCommandHandler : IRequestHandler<LoginCommand, IActionResult>
        {
            private readonly IConfiguration _configuration;
            private readonly AuthenticationContext _context;

            public LoginCommandHandler(IConfiguration configuration, AuthenticationContext context)
            {
                _configuration = configuration;
                _context = context;
            }

            public async Task<IActionResult> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var user = _context.Users.FirstOrDefault(u => u.UserName == request.UserName
                                                 && u.Password == request.Password
                                                 );

                    if (user != null)
                    {
                        var userId = user.Id;
                        var userRoles = (from u in _context.Users
                                         where user.UserName == request.UserName
                                         join role in _context.Roles on u.RoleId equals role.Id
                                         select role.Name).ToList();

                        var tokenGenerator = new GenerateJwtToken(_configuration);

                        var token = tokenGenerator.GenerateToken(userId, request.UserName, userRoles);

                        return new OkObjectResult(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                    return new OkObjectResult("Please check your mail or password again");


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
