using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.API.Fearture.NotificationFearture.Command;
using NotificationService.API.Fearture.NotificationFearture.Queries;

namespace NotificationService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetNotifications(int userId)
        {
            return Ok(await _mediator.Send(new GetNotificationByUserIdQuerry { UserId=userId }));
        }
      
    }
}
