using Contract.Service.Message;
using ForumService.API.Fearture.Command;
using ForumService.API.Fearture.Queries;
using ForumService.API.Feature.Posts.Command;
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

        public async Task<IActionResult> GetAllPost(int page, int pageSize, string? PostTitle)
        {
            return Ok(await _mediator.Send(new GetAllPostQuerry { page = page, pageSize = pageSize, PostTitle = PostTitle }));
        }

        [HttpGet]
        public async Task<IActionResult> GetPostById(int postId)
        {
            try
            {
                var query = new GetPostByIdQuerry { PostId = postId };
                var result = await _mediator.Send(query);

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest(Message.MSG30);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdminPost(CreatAdminPostCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost(int postId, [FromBody] UpdatePostCommand command)
        {
            if (postId != command.PostId)
            {
                return BadRequest(Message.MSG30);
            }

            var result = await _mediator.Send(command);

            return result;
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
