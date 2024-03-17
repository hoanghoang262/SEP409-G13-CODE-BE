using Contract.Service.Message;
using CourseService.API.Feartures.CourseFearture.Command.CreateCourse;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Fearture.Command.Course;
using ModerationService.API.Fearture.Querries.Moderations;
using ModerationService.API.Models;

namespace ModerationService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CourseModerationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Content_ModerationContext _context;

        public CourseModerationController(IMediator mediator, Content_ModerationContext context)
        {
            _mediator = mediator;
            _context = context;

        }

        [HttpPost]
        public async Task<ActionResult> AddCourse(CreateCourseCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCourse(UpdateCourseCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            var command = new DeleteCourseCommand { CourseId = courseId };
            var result = await _mediator.Send(command);

            return Ok(result); 
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseByUserId(int userId)
        {
            var command = new GetCourseByUserIdQuerry { UserId = userId };
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
