using Microsoft.AspNetCore.Mvc;

namespace CompilerService.API.Controllers
{
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}
