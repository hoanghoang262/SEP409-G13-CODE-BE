using Authenticate_Service.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authenticate_Service.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ForgorPasswordController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public ForgorPasswordController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        [HttpGet]
        public async Task<IActionResult> TestEmail()
        {
            var message = new MailRequest
            {
                Body = "<h1>hello</h1>",
                Subject = "test",
                ToAddress = "chienhaviet2408@gmail.com"
            };
            await _emailService.SendEmailAsync(message);  
            return Ok();
        }
    }
}
