using Contract.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotificationService.API.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NotificationService.API.Fearture.NotificationFearture.Queries
{
    public class GetNotificationByUserIdQuerry :IRequest<IActionResult>
    {
        public int UserId { get; set; }
        public int Page { get; set; }   

        public int PageSize { get; set; }

        public class GetNotificationByUserIdQuerryHandler : IRequestHandler<GetNotificationByUserIdQuerry, IActionResult>
        {
            private readonly NotificationContext _context;

            public GetNotificationByUserIdQuerryHandler(NotificationContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Handle(GetNotificationByUserIdQuerry request, CancellationToken cancellationToken)
            {
                var querry = await _context.Notifications.Where(x=>x.RecipientId.Equals(request.UserId)).ToListAsync();
                if (querry == null)
                {
                    return new NotFoundObjectResult("No notification");
                }
                var count = querry.Count();
                var noti = querry
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
                var result = new PageList<Notification>(noti, count, request.Page, request.PageSize);
                return new OkObjectResult(result);
            }
        }
    }
}
