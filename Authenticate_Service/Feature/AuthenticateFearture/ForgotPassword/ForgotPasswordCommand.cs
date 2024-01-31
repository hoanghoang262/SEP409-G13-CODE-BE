using Contract.Service;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authenticate_Service.Feature.AuthenticateFearture.ForgotPassword
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
            public Task<IActionResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
            {
                return null;
              
            }
        }
    }

}
