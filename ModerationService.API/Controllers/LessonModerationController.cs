using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Fearture.Command.Lesson;

namespace ModerationService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LessonModerationController: ControllerBase
    {
        private readonly IMediator _mediator;

        public LessonModerationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateLesson([FromBody] CreateLessonCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLesson(int id, [FromBody] UpdateLessonCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var command = new DeleteLessonCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

