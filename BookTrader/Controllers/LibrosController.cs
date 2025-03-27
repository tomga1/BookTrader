using BookTrader.Data;
using BookTrader.DTOs;
using BookTrader.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper; 

namespace BookTrader.Controllers
{
    public class LibrosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int RegistrosPorPagina = 10;
        private readonly IMapper _mapper;


        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context; 
            _mapper = mapper;   
               
        }

        public async Task<IActionResult> Index(int pagina = 1)
        {
            int totalRegistros = await _context.Libros.CountAsync();
            var libros = await _context.Libros
                .OrderBy(c => c.FechaAgregado) // Ordena por fecha
                .Where(c => c.EstadoPublicacion == EstadoPublicacion.Aprobado)
                .Skip((pagina - 1) * RegistrosPorPagina)
                .Take(RegistrosPorPagina)
                .ToListAsync();


            var modeloPaginado = new PaginacionViewModel<Libros>
            {
                Items = libros,
                PaginaActual = pagina,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / RegistrosPorPagina)

            };

            return View(modeloPaginado);
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromForm] InsertLibroDTO insertLibroDTO)
        {
            if (ModelState.IsValid)
            {
                var libro = _mapper.Map<Libros>(insertLibroDTO);

                _context.Libros.Add(libro);
                await _context.SaveChangesAsync();


                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
