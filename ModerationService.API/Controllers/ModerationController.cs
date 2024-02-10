using CourseService.API.Feartures.CourseFearture.Command.CreateCourse;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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


        //[HttpGet]
        //public async Task<IActionResult> GetAllCourses()
        //{
        //    return Ok(await _mediator.Send(new GetAllCourseQuerry()));
        //}
        //[HttpGet]
        //public async Task<IActionResult> GetCourseByUser(int Id)
        //{
        //    return Ok(await _mediator.Send(new GetCourseByUserIdQuerry { UserId = Id }));
        //}

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



    }
}
