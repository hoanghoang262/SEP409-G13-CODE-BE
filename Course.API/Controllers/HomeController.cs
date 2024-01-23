using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Controllers
{
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}
