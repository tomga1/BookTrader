using Microsoft.AspNetCore.Mvc;

namespace BookTrader.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
