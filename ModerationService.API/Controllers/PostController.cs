using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Fearture.Command.Forum;
using ModerationService.API.Fearture.Querries.Moderations;
using ModerationService.API.Models;

namespace ModerationService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPostById(int postId)
        {

            return Ok(await _mediator.Send(new GetModerationPostByIdQuerry { PostId = postId })); 
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var command = new DeletePostCommand { postId = postId };
            var result = await _mediator.Send(command);

            return Ok(result);
        }

    }
}
