using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Fearture.Command.TheoryQuestion;
using System.Threading.Tasks;

namespace ModerationService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TheoryQuestionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TheoryQuestionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTheoryQuestion([FromBody] CreateTheoryQuestionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTheoryQuestion(int id, [FromBody] UpdateTheoryQuestionCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTheoryQuestion(int id)
        {
            var command = new DeleteTheoryQuestionCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
