
using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Common.DTO;
using ModerationService.API.Fearture.Command.LastExams;
using ModerationService.API.Models;

namespace ModerationService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LastExamController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LastExamController(IMediator mediator) 
        { 
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateLastExam(CreateLastExamCommand command)
        {
            var result = await _mediator.Send(command);

            return result;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLastExam(int id, UpdateLastExamCommand command)
        {
            if (id != command.LastExamId)
            {
                return BadRequest(Message.MSG30);
            }

            return await _mediator.Send(command);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteLastExam(int id)
        {
            var command = new DeleteLastExamCommand { LastExamId = id };
            return await _mediator.Send(command);
        }

        [HttpGet]
        public async Task<ActionResult> GetLastExamById(int id)
        {
            var query = new GetLastExamByIdQuery { LastExamId = id };
            return Ok(await _mediator.Send(query));
        }
    }
}
