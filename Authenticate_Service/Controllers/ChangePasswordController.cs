using Authenticate_Service.Feature.AuthenticateFearture.Command.ChangePassword;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authenticate_Service.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChangePasswordController : ControllerBase
    {
        private readonly IMediator mediator;
        public ChangePasswordController(IMediator _mediator)
        {
            mediator = _mediator;
        }
        [HttpPost]

        public async Task<IActionResult> ChangePass(ChangePasswordCommand command)
        {
            return Ok(await mediator.Send(command));
        }
    }
}
