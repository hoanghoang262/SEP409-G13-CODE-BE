using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumService.API.Controllers
{
    
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}
