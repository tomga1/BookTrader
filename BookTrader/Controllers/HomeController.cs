using BookTrader.Data;
using BookTrader.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BookTrader.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context ,ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context; 
        }

        public async Task<IActionResult> Index()
        {
            var libros = await _context.Libros
                .Where(l => l.EstadoPublicacion == EstadoPublicacion.Aprobado)
                .ToListAsync();

            return View(libros);
        }

        public IActionResult SobreNosotrosView()
        {
            return View();
        }
        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
