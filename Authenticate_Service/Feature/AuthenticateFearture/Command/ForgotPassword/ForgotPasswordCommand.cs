using Contract.Service;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authenticate_Service.Feature.AuthenticateFearture.Command.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<IActionResult>
    {
        public string Email { get; set; }


        public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, IActionResult>
        {
            private readonly IEmailService<MailRequest> _emailService;
            public ForgotPasswordHandler(IEmailService<MailRequest> emailService)
            {
                _emailService = emailService;
            }
            public async Task<IActionResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
            {
                var verificationCode = new Random().Next(100000, 999999).ToString();
                var message = new MailRequest
                {
                    Body = "<h1>your verfication code is :" + verificationCode,
                    ToAddress = request.Email,
                    Subject = "Verify Code"


                };
                await _emailService.SendEmailAsync(message);

                return null;

            }
        }
    }

}
