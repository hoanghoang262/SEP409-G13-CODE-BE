using ForumService.API.Fearture.Command;
using ForumService.API.Fearture.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ForumController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ForumController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllPost()
        {

            return Ok(await _mediator.Send(new GetAllPostQuerry()));

        }
        [HttpPost]
        public async Task<IActionResult> CreateAdminPost(CreatAdminPostCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpGet]
        public async Task<IActionResult> GetPostById(int postId)
        {
            try
            {
                var query = new GetPostByIdQuerry { PostId = postId };
                var result = await _mediator.Send(query);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting post by id: {ex.Message}");
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var command = new DeletePostCommand { PostId = postId };
            var result = await _mediator.Send(command);

            return result;
        }
    }
}
