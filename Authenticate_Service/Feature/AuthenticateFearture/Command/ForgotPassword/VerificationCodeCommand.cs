using Authenticate_Service.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authenticate_Service.Feature.AuthenticateFearture.Command.ForgotPassword
{
    public class VerificationCodeCommand : IRequest<IActionResult>
    {
        public string? Code { get; set; }
        public string? Email { get; set; }

        public class VerificationCodeHandler : IRequestHandler<VerificationCodeCommand, IActionResult>
        {
            
            private readonly AuthenticationContext _context;
            public VerificationCodeHandler( AuthenticationContext context)
            {
               
                _context = context;
            }
            public async Task<IActionResult> Handle(VerificationCodeCommand request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(request.Email) &&
                                                                          u.VerificationCode.Equals(request.Code));
                if (user == null)
                {
                    return new BadRequestObjectResult("Please check your password reset code again");
                }

                return new OkObjectResult("Verified successfully");
            }
        }
    }
}
