using Authenticate_Service.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authenticate_Service.Feature.AuthenticateFearture.Command.ChangePassword
{
    public class ChangePasswordCommand : IRequest<IActionResult>
    {
        public string Email { get; set; } 
   
        public string NewPassword { get; set; }

        public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, IActionResult>
        {
            private readonly AuthenticationContext _context;

            public ChangePasswordHandler(AuthenticationContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
                if (user == null)
                {
                    return new BadRequestResult();
                }
                user.Password = request.NewPassword;

                await _context.SaveChangesAsync();

                return new OkObjectResult("Mật khẩu đã được thay đổi thành công.");
            }
        }
    }
}
