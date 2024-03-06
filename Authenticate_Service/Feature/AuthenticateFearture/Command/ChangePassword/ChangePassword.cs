using Authenticate_Service.Common;
using Authenticate_Service.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authenticate_Service.Feature.AuthenticateFearture.Command.ChangePassword
{
    public class ChangePasswordCommand : IRequest<IActionResult>
    {
        public string Email { get; set; } 

        public string OldPassword { get; set; } 
   
        public string? NewPassword { get; set; }

        public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, IActionResult>
        {
            private readonly AuthenticationContext _context;
            private readonly HassPaword hash = new HassPaword();

            public ChangePasswordHandler(AuthenticationContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == request.Email&& u.Password==request.OldPassword);
                if (user == null)
                {
                    return new BadRequestObjectResult("Check your email or password again");
                }
               
                user.Password =request.NewPassword;

                await _context.SaveChangesAsync();

                return new OkObjectResult("Your password has been successfully changed " );
            }
        }
    }
}
