using CourseService.API.Common.Mapping;
using EventBus.Message.Event;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationService.API.Models;

namespace NotificationService.API.Fearture.NotificationFearture.Command
{
    public class SendNotificationCommand : IRequest<IActionResult>, IMapFrom<NotificationEvent>
    {
        public int NotificationsId { get; set; }
        public int RecipientId { get; set; }
        public string? NotificationContent { get; set; }

        public DateTime SendDate { get; set; }

        public bool IsSeen { get; set; }

        public class SendNotificationCommandHandle : IRequestHandler<SendNotificationCommand, IActionResult>
        {
            private readonly NotificationContext _context;

            public SendNotificationCommandHandle(NotificationContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
            {
                var notification = new Notification
                {
                    IsSeen=false,
                    NotificationContent = request.NotificationContent,
                   RecipientId=request.RecipientId,
                   SendDate=DateTime.UtcNow,
                };
                 _context.Notifications.Add(notification);    
                await _context.SaveChangesAsync();
                return new OkObjectResult(notification);
            }
        }
    }
}
