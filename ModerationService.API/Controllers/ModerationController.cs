using CourseService.API.Feartures.CourseFearture.Command.CreateCourse;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Fearture.Command;
using ModerationService.API.Fearture.Command.Moderation;
using ModerationService.API.Models;

namespace ModerationService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ModerationController : ControllerBase
    {
        private readonly IMediator _mediator;
     

        public ModerationController(IMediator mediator)
        {
            _mediator = mediator;
          
        }


        [HttpPost]
        public async Task<ActionResult> AddCourse(CreateCourseCommand command)
        {

            return Ok(await _mediator.Send(command));
        }
        [HttpPost]
        public async Task<ActionResult> ModerationCourse(int courseId)
        {

            return Ok(await _mediator.Send(new ModerationCourseCommand { CourseId=courseId}));
        }
        [HttpPost]

        public async Task<ActionResult> UpdateCourse(UpdateCourseCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost]

        public async Task<ActionResult> CreateContentForum(CreateForumCommand command)
        {
            return Ok(await _mediator.Send(command));
        }



    }
}
