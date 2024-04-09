using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Fearture.Command;
using ModerationService.API.Fearture.Command.Forum;
using ModerationService.API.Fearture.Querries.Moderations;
using ModerationService.API.Fearture.Querries.Post;
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
            var command = new DeletePostCommand { PostId = postId };
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(Message.MSG30);
            }

            return Ok(await _mediator.Send(command));
        }
        [HttpGet]
        public async Task<IActionResult> GetPostsByUserId(int userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string postTitle = null)
        {
            var query = new GetPostByUserIdQuerry
            {
                UserId = userId,
                Page = page,
                PageSize = pageSize,
                PostTitle = postTitle
            };

            var result = await _mediator.Send(query);

            return result;
        }

    }
}
