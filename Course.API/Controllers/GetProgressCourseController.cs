using CourseService.API.Feartures.CourseFearture.Queries.CourseQueries;
using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GetProgressCourseController : ControllerBase
    {
        private readonly IMediator _mediator;
        public GetProgressCourseController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]

        public async Task<IActionResult> GetProgress(int userId) {

            return Ok(await _mediator.Send(new GetProcessCourseQuerry {UserId=userId}));
        }
    }
}
