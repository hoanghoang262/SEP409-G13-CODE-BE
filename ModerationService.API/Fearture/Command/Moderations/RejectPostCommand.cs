using Contract.Service.Message;
using EventBus.Message.Event;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Command.Moderations
{
    public class RejectPostCommand : IRequest<IActionResult>
    {
        public int PostId { get; set; }
        public string ReasonWhyReject { get; set; }

        public class RejectPostCommandHandler : IRequestHandler<RejectPostCommand, IActionResult>
        {
            private readonly Content_ModerationContext _moderationContext;
            private readonly IPublishEndpoint _publish;

            public RejectPostCommandHandler(Content_ModerationContext moderationContext, IPublishEndpoint publish)
            {
                _moderationContext = moderationContext;
                _publish = publish;
            }
            public async Task<IActionResult> Handle(RejectPostCommand request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.ReasonWhyReject))
                {
                    return new BadRequestObjectResult(Message.MSG11);
                }

                var moder = await _moderationContext.Moderations.FirstOrDefaultAsync(c => c.PostId.Equals(request.PostId));
                if (moder == null)
                {
                    return new BadRequestObjectResult(Message.MSG30);
                }

                _moderationContext.Moderations.Remove(moder);
                await _moderationContext.SaveChangesAsync();

                var notificationForAdminBussiness = new NotificationEvent
                {
                    RecipientId = moder.CreatedBy,
                    IsSeen = false,
                    NotificationContent = request.ReasonWhyReject,
                    SendDate = DateTime.Now,
                   Post_Id = (int)moder.PostId,
                };
                await _publish.Publish(notificationForAdminBussiness);

                return new OkObjectResult(Message.MSG16);
            }
        }
    }
}
