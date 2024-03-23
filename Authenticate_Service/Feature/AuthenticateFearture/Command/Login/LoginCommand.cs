using Authenticate_Service.Common;
using Authenticate_Service.Models;
using Contract.Service.Message;
using Google;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

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
                        var userRoles = (from u in _context.Users
                                         join role in _context.Roles on u.RoleId equals role.Id
                                         where u.UserName == request.UserName
                                         select role.Name).ToList();

                        var tokenGenerator = new GenerateJwtToken(_configuration);
                        var token = tokenGenerator.GenerateToken(userId, request.UserName,user.ProfilePict, userRoles);

                        return new OkObjectResult(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            Message.MSG02
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
