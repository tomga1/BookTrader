using Microsoft.AspNetCore.Mvc;

namespace BookTrader.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
