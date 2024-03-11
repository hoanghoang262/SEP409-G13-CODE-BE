using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Fearture.Command;

namespace ModerationService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChapterModerationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChapterModerationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> AddChapter([FromBody] CreateChapterCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateChapter(int id, [FromBody] UpdateChapterCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteChapter(int id)
        {
            var command = new DeleteChapterCommand { ChapterId = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
