using Authenticate_Service.Models;
using Contract.Service;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authenticate_Service.Feature.AuthenticateFearture.Command.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<IActionResult>
    {
        public string Email { get; set; }
        public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, IActionResult>
        {
            private readonly IEmailService<MailRequest> _emailService;
            private readonly AuthenticationContext _context;
            public ForgotPasswordHandler(IEmailService<MailRequest> emailService,AuthenticationContext context)
            {
                _emailService = emailService;
                _context = context;
            }
            public async Task<IActionResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u=>u.Email.Equals(request.Email));
                if (user == null) {
                    return new BadRequestObjectResult("Please check your email");
                }
                var verificationCode = new Random().Next(100000, 999999).ToString();
                user.VerificationCode=verificationCode;
                await _context.SaveChangesAsync();
                var message = new MailRequest
                {
                    Body = "<h1>your verfication code is :" + verificationCode,
                    ToAddress = request.Email,
                    Subject = "Verify Code"
                };
              await  _emailService.SendEmailAsync(message);

                return new OkObjectResult("We have sent the confirmation code to your email: "+request.Email);

            }
        }
    }

}
