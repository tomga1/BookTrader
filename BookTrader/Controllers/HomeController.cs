using BookTrader.Data;
using BookTrader.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<IActionResult> Index(string searchString, int pagina = 1)
        {
            int registrosPorPagina = 24;

            var libros = _context.Libros
                .Include(l => l.Categoria)  // Asegúrate de incluir la propiedad de navegación 'Categoria'
                .Include(l => l.Idioma)
                .Include(l => l.Condicion)
                .Where(l => l.EstadoPublicacion == EstadoPublicacion.Aprobado);

            if(!string.IsNullOrEmpty(searchString))
            {
                libros = libros.Where(n => n.Nombre.Contains(searchString) || n.Autor.Contains(searchString));
            }

            int totalRegistros = await libros.CountAsync();



            var libros2 = await libros
            .OrderBy(l => l.Nombre) // O el orden que prefieras
            .Skip((pagina - 1) * registrosPorPagina)
            .Take(registrosPorPagina)
            .ToListAsync();

            var modeloPaginado = new PaginacionViewModel<Libros>
            {
                Items = libros2,
                PaginaActual = pagina,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina)
            };

            return View(modeloPaginado);
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
