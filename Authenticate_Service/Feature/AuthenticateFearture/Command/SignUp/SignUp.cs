using Authenticate_Service.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authenticate_Service.Feature.AuthenticateFearture.Command.SignUp
{
    public class SignUpCommand : IRequest<IActionResult>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? UserName { get; set; }

        public class SignUpCommandHandler : IRequestHandler<SignUpCommand, IActionResult>
        {
            private readonly AuthenticationContext context;

            public SignUpCommandHandler(AuthenticationContext _context)
            {
                context= _context;
            }

            public async Task<IActionResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
            {
                if (context.Users.Any(u => u.Email == request.Email))
                {
                    return new BadRequestObjectResult("A user is already registered with this e-mail address.");
                }
                if (context.Users.Any(u => u.UserName == request.UserName))
                {
                    return new BadRequestObjectResult("A user is already registered with this username.");
                }
                var newUser = new User { Email = request.Email, UserName = request.UserName,Password=request.Password,RoleId=1 };
                

               
                context.Users.Add(newUser);
                await context.SaveChangesAsync();

               
                return new OkObjectResult("Đăng ký thành công.");

            }
        }
    }
}
