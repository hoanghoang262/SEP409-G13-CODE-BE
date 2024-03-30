using Authenticate_Service.Common;
using Authenticate_Service.Models;
using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Authenticate_Service.Feature.AuthenticateFearture.Command.ChangePassword
{
    public class ChangePasswordCommand : IRequest<IActionResult>
    {
        public string Email { get; set; }
        public string? OldPassword { get; set; }
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
                // Validate input
                if (string.IsNullOrEmpty(request.OldPassword) || string.IsNullOrEmpty(request.NewPassword))
                {
                    return new BadRequestObjectResult(Message.MSG11);
                }

                // Validate password
                string pattern = "^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*()-_+=])[A-Za-z0-9!@#$%^&*()-_+=]{8,32}$";
                Regex regex = new Regex(pattern);
                if (!regex.IsMatch(request.NewPassword))
                {
                    return new BadRequestObjectResult(Message.MSG17);
                }

                // Check user old password
                var user = _context.Users
                    .FirstOrDefault(u => u.Email == request.Email && u.Password == request.OldPassword);
                if (user == null)
                {
                    return new BadRequestObjectResult(Message.MSG13);
                }

                user.Password = request.NewPassword;
                await _context.SaveChangesAsync();

                return new OkObjectResult(Message.MSG12);
            }
        }
    }
}
