
using Authenticate_Service.Feature.AuthenticateFearture.Command.ForgotPassword;
using Contract.Service;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authenticate_Service.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ForgorPasswordController : ControllerBase
    {
        private readonly IEmailService<MailRequest> _emailService;
        private readonly IMediator _mediator;

        public ForgorPasswordController(IEmailService<MailRequest> emailService, IMediator mediator)
        {
            _emailService = emailService;
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> TestEmail(ForgotPasswordCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        
    }
}
