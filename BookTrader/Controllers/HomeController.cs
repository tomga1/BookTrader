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

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, int pagina = 1)
        {
            int registrosPorPagina = 16;

            var libros = _context.Libros
                .Include(l => l.Categoria)  // Incluye la información de la categoría
                .Include(l => l.Idioma)
                .Include(l => l.Condicion)
                .Include(l => l.Formato)
                .Include(l => l.EstadoPublicacion)
                .Include(l => l.Publicador)
                    .ThenInclude(u => u.Localidad)
                        .ThenInclude(loc => loc.Provincia)
                            .ThenInclude(p => p.Pais)
                .Where(l => l.EstadoPublicacion.Nombre == "Aprobado" );

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

        [Authorize(Roles = "Admin")]
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


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult FiltrarLibrosPorCategorias([FromBody] List<int> subcategoriasIds)
        {
            IQueryable<Libros> query = _context.Libros
                .Include(l => l.Categoria)
                .Include(l => l.SubCategorias)
                .Include(l => l.Condicion)
                .Include(l => l.Publicador)
                    .ThenInclude(p => p.Localidad)
                        .ThenInclude(l => l.Provincia)
                            .ThenInclude(p => p.Pais);


            if (subcategoriasIds != null && subcategoriasIds.Any())
            {
                query = query.Where(l => subcategoriasIds.Contains(l.SubCategoriasId));
            }

            var librosFiltrados = query.ToList();

            var modeloPaginado = new PaginacionViewModel<Libros>
            {
                Items = librosFiltrados,
                PaginaActual = 1,
                TotalPaginas = 1 // o lo calculás si hacés paginación en el filtro también
            };

            var viewModel = new HomeViewModel
            {
                LibrosPaginados = modeloPaginado
            };

            return PartialView("_LibrosFiltradosPartial", viewModel);


        }
    }
}
