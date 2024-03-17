using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Fearture.Command.PracticeQuestion;
using ModerationService.API.Fearture.Querries.PracticeQuestion;
using ModerationService.API.Feature.Command;

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

        [HttpPut]
        public async Task<ActionResult<PracticeQuestionDTO>> UpdatePracticeQuestion(int id, UpdatePracticeQuestionCommand command)
        {
            if (id != command.PracticeQuestionId)
            {
                return BadRequest(Message.MSG30);
            }

            var practiceQuestionDTO = await _mediator.Send(command);

            return Ok(practiceQuestionDTO);
        }

        [HttpGet]
        public async Task<ActionResult<PracticeQuestionDTO>> GetPracticeQuestionById(int id)
        {
            var query = new GetPracticeQuestionByIdQuery { PracticeQuestionId = id };
            var practiceQuestionDTO = await _mediator.Send(query);

            return Ok(practiceQuestionDTO);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePracticeQuestion(int id)
        {
            var command = new DeletePracticeQuestionCommand { PracticeQuestionId = id };
            var success = await _mediator.Send(command);
            return Ok(success);
        }
    }
}
