
using Contract.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authenticate_Service.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ForgorPasswordController : ControllerBase
    {
        private readonly IEmailService<MailRequest> _emailService;

        public ForgorPasswordController(IEmailService<MailRequest> emailService)
        {
            _emailService = emailService;
        }
        [HttpGet]
        public async Task<IActionResult> TestEmail(string Email)
        {
            var message = new MailRequest
            {
                Body = "<h1>hello</h1>",
                Subject = "test",
                ToAddress = Email,
            };
            await _emailService.SendEmailAsync(message);  
            return Ok();
        }
    }
}
