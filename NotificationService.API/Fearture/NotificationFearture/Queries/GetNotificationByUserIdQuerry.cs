using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotificationService.API.Models;

namespace NotificationService.API.Fearture.NotificationFearture.Queries
{
    public class GetNotificationByUserIdQuerry :IRequest<IActionResult>
    {
        public int UserId { get; set; }

        public class GetNotificationByUserIdQuerryHandler : IRequestHandler<GetNotificationByUserIdQuerry, IActionResult>
        {
            private readonly NotificationContext _context;

            public GetNotificationByUserIdQuerryHandler(NotificationContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Handle(GetNotificationByUserIdQuerry request, CancellationToken cancellationToken)
            {
                var query= await _context.Notifications.Where(x=>x.RecipientId.Equals(request.UserId)).ToListAsync();
                if (query==null)
                {
                    return new NotFoundObjectResult("No notification");
                }
                return new OkObjectResult(query);
            }
        }
    }
}
