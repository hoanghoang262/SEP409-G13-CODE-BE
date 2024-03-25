using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}
