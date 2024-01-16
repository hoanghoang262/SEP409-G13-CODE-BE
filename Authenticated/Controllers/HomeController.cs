using Microsoft.AspNetCore.Mvc;

namespace Authenticated.Controllers
{
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}
