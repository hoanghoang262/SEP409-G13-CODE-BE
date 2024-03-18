using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModerationService.API.Models;

namespace ModerationService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LastExamController : ControllerBase
    {
        private readonly Content_ModerationContext _context;
        public LastExamController(Content_ModerationContext context) 
        { 
            _context = context;
        }
        //[HttpPost]


    }
}
