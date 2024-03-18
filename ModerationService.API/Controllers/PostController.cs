using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<Post>> GetPostById(int postId)
        {
           
            var post = await _mediator.Send(new GetModerationPostByIdQuerry { PostId=postId});


            return Ok(post); 
        }

    }
}
