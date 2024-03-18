using CourseService.API.Feartures.CourseFearture.Command.Notes;
using CourseService.API.Feartures.CourseFearture.Queries.Notes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NoteController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateNote(CreateNoteCommand command)
        {
            var noteId = await _mediator.Send(command);
            return Ok(noteId);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNote(int id, UpdateNoteCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteNote(int id)
        {
            await _mediator.Send(new DeleteNoteCommand { Id = id });
            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNoteOfUser(int userId, int lessonId)
        {
            var query = new GetAllNoteOfUserQuerry { UserId = userId, LessonId = lessonId };
            var notes = await _mediator.Send(query);

            if (notes == null)
            {
                return NotFound();
            }

            return Ok(notes);
        }
    }
}
