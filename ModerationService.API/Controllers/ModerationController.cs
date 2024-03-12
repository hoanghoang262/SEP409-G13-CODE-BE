using CourseGRPC;
using CourseService.API.Feartures.CourseFearture.Command.CreateCourse;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Fearture.Command;
using ModerationService.API.Fearture.Command.Moderations;
using ModerationService.API.Fearture.Querries.Moderations;
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
        [HttpGet]

        public async Task<IActionResult> GetModerationCourseById(int courseId)
        {
            var query = new GetModerationCourseByIdQuerry { CourseId = courseId };
            var result = await _mediator.Send(query);

            return result;
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

        public async Task<ActionResult> CreatePost(CreatePostCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetModerationsCourse(string? courseName,int page = 1, int pageSize = 5)
        {
            try
            {
                var query = new GetModerationCourseQuerry { Page = page, PageSize = pageSize,CourseName=courseName };
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
        [HttpGet]
        public async Task<IActionResult> GetModerationsPost(string? postTitle, int page = 1, int pageSize = 5)
        {
            try
            {
                var query = new GetModerationPostQuerry { Page = page, PageSize = pageSize, PostTitle = postTitle };
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

        [HttpPost]
        public async Task<ActionResult> SendToModeration(int CourseId)
        {
            var course = await _context.Courses.FindAsync(CourseId);

            course.IsCompleted = true;

            await _context.SaveChangesAsync();

            return Ok("Send successfully");
            
        }


    }
}
