using BookTrader.Data;
using BookTrader.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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

        public async Task<IActionResult> Index(string searchString)
        {
            var libros = await _context.Libros
                .Include(l => l.Categoria)  // Aseg�rate de incluir la propiedad de navegaci�n 'Categoria'
                .Where(l => l.EstadoPublicacion == EstadoPublicacion.Aprobado)
                .ToListAsync();

            if(!string.IsNullOrEmpty(searchString))
            {
                libros = libros.Where(n => n.Nombre.Contains(searchString) || n.Autor.Contains(searchString)).ToList();
            }

            return View(libros);
        }


        public IActionResult SobreNosotrosView()
        {
            return View();
        }

        [Authorize] 
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public IActionResult User()
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
