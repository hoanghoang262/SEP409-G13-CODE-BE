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

        public async Task<IActionResult> GetAllPost(string? PostTitle,int page=1,int pageSize=5)
        {

            return Ok(await _mediator.Send(new GetAllPostQuerry { Page=page,PageSize=pageSize,PostTitle=PostTitle}));

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
        [HttpPut]
        public async Task<IActionResult> UpdatePost(int postId, [FromBody] UpdatePostCommand command)
        {
            if (postId != command.PostId)
            {
                return BadRequest("PostId in request body does not match the PostId in the URL.");
            }

            var result = await _mediator.Send(command);

            return result;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPostsByUserId(int userId,  int page = 1, int pageSize = 5)
        {
            var query = new GetAllPostByUserId
            {
                UserId = userId,
                page = page,
                pageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return result;
        }
    }
}
