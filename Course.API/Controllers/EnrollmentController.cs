using Contract.Service.Message;
using CourseService.API.Feartures.CourseFearture.Queries.CourseQueries;
using CourseService.API.Feartures.EnrollmentFeature.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EnrollmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEnrollment([FromBody] CreateEnrollmentCourseCommand command)
        {
            try
            {
                var enrollmentId = await _mediator.Send(command);
                return Ok(enrollmentId);
            }
            catch (Exception)
            {
                return BadRequest(Message.MSG30);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseUserEnrollment(int courseId, int userId)
        {
            var query = new GetCourseUserEnrollQuerry { CourseId = courseId, UserId = userId };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
