
using Authenticate_Service.Feature.AuthenticateFearture.Command.ForgotPassword;
using Contract.Service;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authenticate_Service.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ForgortPasswordController : ControllerBase
    {
        private readonly IEmailService<MailRequest> _emailService;
        private readonly IMediator _mediator;

        public ForgortPasswordController(IEmailService<MailRequest> emailService, IMediator mediator)
        {
            _emailService = emailService;
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command)
        {
            return Ok(await _mediator.Send(command));
        }    
        [HttpPost]
        public async Task<IActionResult> VerificationCode(VerificationCodeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        
    }
}
