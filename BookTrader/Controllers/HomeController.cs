using BookTrader.Data;
using BookTrader.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BookTrader.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly UserManager<Users> _userManager;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, IMemoryCache memoryCache, UserManager<Users> userManager)
        {
            _logger = logger;
            _context = context;
            _memoryCache = memoryCache;
            _userManager = userManager; // ? Correcto
        }


        public async Task<IActionResult> Index(string searchString, int pagina = 1)
        {
            int registrosPorPagina = 16;

            string cacheKey = $"libros_home_page_{pagina}";

            List<Libros> libros2;
            int totalRegistros;

            if (string.IsNullOrEmpty(searchString))
            {
                if (!_memoryCache.TryGetValue(cacheKey, out libros2))
                {
                    var query = _context.Libros
                        .Include(l => l.Categoria)
                        .Include(l => l.Idioma)
                        .Include(l => l.Condicion)
                        .Include(l => l.Formato)
                        .Include(l => l.EstadoPublicacion)
                        .Include(l => l.Publicador)
                            .ThenInclude(u => u.Localidad)
                                .ThenInclude(loc => loc.Provincia)
                                    .ThenInclude(p => p.Pais)
                        .Where(l => l.EstadoPublicacion.Nombre == "Aprobado");

                    totalRegistros = await query.CountAsync();

                    libros2 = await query
                        .OrderBy(l => l.Nombre)
                        .Skip((pagina - 1) * registrosPorPagina)
                        .Take(registrosPorPagina)
                        .ToListAsync();

                    // Cachea los resultados solo si es búsqueda vacía
                    _memoryCache.Set(cacheKey, libros2, TimeSpan.FromMinutes(5));
                    _memoryCache.Set($"{cacheKey}_total", totalRegistros, TimeSpan.FromMinutes(5));
                }
                else
                {
                    totalRegistros = _memoryCache.Get<int>($"{cacheKey}_total");
                }
            }
            else
            {
                var query = _context.Libros
                    .Include(l => l.Categoria)
                    .Include(l => l.Idioma)
                    .Include(l => l.Condicion)
                    .Include(l => l.Formato)
                    .Include(l => l.EstadoPublicacion)
                    .Include(l => l.Publicador)
                        .ThenInclude(u => u.Localidad)
                            .ThenInclude(loc => loc.Provincia)
                                .ThenInclude(p => p.Pais)
                    .Where(l => l.EstadoPublicacion.Nombre == "Aprobado");

                query = query.Where(n => n.Nombre.Contains(searchString) || n.Autor.Contains(searchString));

                totalRegistros = await query.CountAsync();

                libros2 = await query
                    .OrderBy(l => l.Nombre)
                    .Skip((pagina - 1) * registrosPorPagina)
                    .Take(registrosPorPagina)
                    .ToListAsync();
            }

            var categorias = await _memoryCache.GetOrCreateAsync("categorias_menu", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await _context.Categorias
                    .Include(c => c.SubCategorias)
                    .ToListAsync();
            });

            var modeloPaginado = new PaginacionViewModel<Libros>
            {
                Items = libros2,
                PaginaActual = pagina,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina)
            };

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
        public IActionResult PerfilUsuario()
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
