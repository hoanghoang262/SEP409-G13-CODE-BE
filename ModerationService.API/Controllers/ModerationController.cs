using CourseGRPC;
using CourseService.API.Feartures.CourseFearture.Command.CreateCourse;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Fearture.Command;
using ModerationService.API.Fearture.Command.Moderations;
using ModerationService.API.Feature.Queries;
using ModerationService.API.Models;

namespace ModerationService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ModerationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Content_ModerationContext _context;

        public ModerationController(IMediator mediator,Content_ModerationContext context)
        {
            _mediator = mediator;
            _context= context;
          
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
        public async Task<ActionResult> ModerationPost(int postId)
        {

            return Ok(await _mediator.Send(new ModerationPostCommand { PostId = postId }));
        }
        [HttpPost]

        public async Task<ActionResult> UpdateCourse(UpdateCourseCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost]

        public async Task<ActionResult> CreateContentForum(CreatePostCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetModerations(int page = 1, int pageSize = 5)
        {
            try
            {
                var query = new GetModerationQuerry { Page = page, PageSize = pageSize };
                var result = await _mediator.Send(query);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
       



    }
}
