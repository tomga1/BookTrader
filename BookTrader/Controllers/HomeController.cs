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
            int registrosPorPagina = 16;

            // Consulta para obtener los libros aprobados
            var libros = _context.Libros
                .Include(l => l.Categoria)  // Incluye la información de la categoría
                .Include(l => l.Idioma)
                .Include(l => l.Condicion)
                .Where(l => l.EstadoPublicacion == EstadoPublicacion.Aprobado);

            if (!string.IsNullOrEmpty(searchString))
            {
                libros = libros.Where(n => n.Nombre.Contains(searchString) || n.Autor.Contains(searchString));
            }

            int totalRegistros = await libros.CountAsync();

            var libros2 = await libros
                .OrderBy(l => l.Nombre) // Ordena según la lógica que requieras
                .Skip((pagina - 1) * registrosPorPagina)
                .Take(registrosPorPagina)
                .ToListAsync();

            // Obtén la lista de categorías y sus subcategorías
            var categorias = await _context.Categorias
                .Include(c => c.SubCategorias)  // Asegúrate de que la entidad Categoria tenga la propiedad de navegación SubCategorias
                .ToListAsync();

            // Crea el modelo de paginación para los libros
            var modeloPaginado = new PaginacionViewModel<Libros>
            {
                Items = libros2,
                PaginaActual = pagina,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina)
            };

            // Crea el HomeViewModel combinando ambas colecciones
            var viewModel = new BookTrader.Models.HomeViewModel
            {
                LibrosPaginados = modeloPaginado,
                Categorias = categorias
            };

            return View(viewModel);
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
