using EventBus.Message.Event;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.Moderations
{
    public class ModerationPostCommand : IRequest<IActionResult>
    {
        public int PostId { get; set; }


        public class ModerationPostCommandHandler : IRequestHandler<ModerationPostCommand, IActionResult>
        {
            private readonly IPublisher _publish;
            private readonly Content_ModerationContext _context;
            public ModerationPostCommandHandler(IPublisher publish,Content_ModerationContext context)
            {
                _publish = publish;
                _context = context;
            }
            public async  Task<IActionResult> Handle(ModerationPostCommand request, CancellationToken cancellationToken)
            {
               var post = _context.Posts.FirstOrDefault(c=>c.Id.Equals(request.PostId));
                var postEvent = new PostEvent
                {
                    Id = post.Id,
                    CreatedBy = post.CreatedBy,
                    Description = post.Description,
                    LastUpdate = post.LastUpdate,
                    PostContent= post.PostContent,
                    Title = post.Title  
                };
                await _publish.Publish(postEvent);
                return  new OkObjectResult (postEvent);

            }
        }
    }
}
