using EventBus.Message.Event;
using ForumService.API.Models;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumService.API.Fearture.Command
{
    public class DeletePostCommand : IRequest<IActionResult>
    {
        public int PostId { get; set; }


        public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, IActionResult>
        {
            private readonly ForumContext _context;
            private readonly IPublishEndpoint publish;
            public DeletePostCommandHandler(ForumContext context,IPublishEndpoint _publish)
            {
                _context = context;
                publish = _publish;
            }
            public async Task<IActionResult> Handle(DeletePostCommand request, CancellationToken cancellationToken)
            {
                var post = await _context.Posts.FirstOrDefaultAsync(c => c.Id.Equals(request.PostId));
                var postIdEvent = new PostIdEvent { postId = request.PostId };
                if (post == null)
                {
                    return new BadRequestObjectResult("Not found");
                }
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                await publish.Publish(postIdEvent);

                return new OkObjectResult("Delete succesfully");
            }
        }
    }
}
