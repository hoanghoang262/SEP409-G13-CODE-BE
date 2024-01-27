using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authenticate_Service.Feature.AuthenticateFearture.ForgotPassword
{
    public class ForgotPasswordCommand: IRequest<IActionResult>
    {
        public string? Email {  get; set; }

        public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, IActionResult>
        {
            public Task<IActionResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }

}
