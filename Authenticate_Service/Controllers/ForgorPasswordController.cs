
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

            return Ok();
        }
    }
}
