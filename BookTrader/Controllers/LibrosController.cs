using BookTrader.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookTrader.Controllers
{
    public class LibrosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LibrosController(ApplicationDbContext context)
        {
            _context = context; 
            
        }

        public IActionResult Index()
        {   
            return View();
        }
    }
}
