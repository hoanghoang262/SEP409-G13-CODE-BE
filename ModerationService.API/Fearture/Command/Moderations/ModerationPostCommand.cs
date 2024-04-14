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

              

                var moderation = _context.Moderations.FirstOrDefault(c => c.PostId.Equals(request.PostId));
                moderation.Status = "Approve";
                _context.Moderations.Remove(moderation);
                await _context.SaveChangesAsync();
                var notification = new NotificationPostEvent
                {
                    RecipientId = moderation.CreatedBy,
                    IsSeen = false,
                    NotificationContent = "Bài đăng của bạn đã được phê duyệt",
                    SendDate = DateTime.Now,
                    Post_Id = request.PostId,
                };
                await _publish.Publish(notification);
                await Task.Delay(3500);

                return new OkObjectResult(notification);
            }
        }
    }
}
