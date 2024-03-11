using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Fearture.Command;

namespace ModerationService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PracticeQuestionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PracticeQuestionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePracticeQuestion([FromBody] CreatePracticeQuestionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
