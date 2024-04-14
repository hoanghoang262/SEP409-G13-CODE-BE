using CourseService.API.Common.Mapping;
using EventBus.Message.Event;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationService.API.Models;

namespace NotificationService.API.Fearture.NotificationFearture.Command
{
    public class CreateNotificationPostCommand : IRequest<IActionResult>, IMapFrom<NotificationPostEvent>
    {
        public int? RecipientId { get; set; }
        public string? NotificationContent { get; set; }
        public int Post_Id { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsSeen { get; set; }

        public class CreateNotificationPostCommandHandle : IRequestHandler<CreateNotificationPostCommand, IActionResult>
        {
            private readonly NotificationContext _context;

            public CreateNotificationPostCommandHandle(NotificationContext context)
            {
                _context = context;
            }
            public async  Task<IActionResult> Handle(CreateNotificationPostCommand request, CancellationToken cancellationToken)
            {
                var notification = new Notification
                {
                    IsSeen=false,
                    NotificationContent = request.NotificationContent,
                   RecipientId=request.RecipientId,
                   SendDate=DateTime.UtcNow,
                   PostId=request.Post_Id,
                };
                 _context.Notifications.Add(notification);    
                 _context.SaveChanges();
                return new OkObjectResult(notification);
            }
        }
    }
}
