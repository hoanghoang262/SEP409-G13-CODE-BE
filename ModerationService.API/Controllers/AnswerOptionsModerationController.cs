using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Fearture.Command;
using ModerationService.API.Fearture.Command.AnswerOptions;
using ModerationService.API.Fearture.Command.TheoryQuestion;

namespace ModerationService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnswerOptionsModerationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AnswerOptionsModerationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAnswerOptions([FromBody] CreateAnswerOptionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateTheoryQuestion(int id, [FromBody] UpdateTheoryQuestionCommand command)
        //{
        //    if (id != command.Id)
        //    {
        //        return BadRequest();
        //    }

        //    var result = await _mediator.Send(command);
        //    return Ok(result);
        //}

        [HttpDelete]
        public async Task<IActionResult> DeleteAnswerOptions([FromBody] DeleteAnswerOptionCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }
    }
}
