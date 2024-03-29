using Contract.Service.Message;
using EventBus.Message.Event;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.Moderations
{
    public class ModerationPostCommand : IRequest<IActionResult>
    {
        public int PostId { get; set; }

        public class ModerationPostCommandHandler : IRequestHandler<ModerationPostCommand, IActionResult>
        {
            private readonly IPublishEndpoint _publish;
            private readonly Content_ModerationContext _context;
            public ModerationPostCommandHandler(IPublishEndpoint publish, Content_ModerationContext context)
            {
                _publish = publish;
                _context = context;
            }
            public async Task<IActionResult> Handle(ModerationPostCommand request, CancellationToken cancellationToken)
            {
                // Check if post exists
                var post = _context.Posts.FirstOrDefault(c => c.Id.Equals(request.PostId));
                if (post == null)
                {
                    return new NotFoundObjectResult(Message.MSG34);
                }

                var postEvent = new PostEvent
                {
                    Id = post.Id,
                    CreatedBy = (int)post.CreatedBy,
                    Description = post.Description,
                    LastUpdate = post.LastUpdate,
                    PostContent = post.PostContent,
                    Title = post.Title
                };

                var moderation = _context.Moderations.FirstOrDefault(c => c.PostId.Equals(request.PostId));
                moderation.Status = "Approve";
                _context.Moderations.Remove(moderation);
                await _context.SaveChangesAsync();
                await _publish.Publish(postEvent);

                return new OkObjectResult(postEvent);
            }
        }
    }
}
