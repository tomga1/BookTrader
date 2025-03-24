using BookTrader.Data;
using BookTrader.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AutoMapper;
using BookTrader.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BookTrader.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int RegistrosPorPagina = 10; 
        private readonly IMapper _mapper;

        public CategoriasController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper; 
        }

        public async Task<IActionResult> Index(int pagina = 1)
        {
            int totalRegistros = await _context.Categorias.CountAsync();
            var categorias = await _context.Categorias
                .OrderBy(c => c.FechaAgregado) // Ordena por fecha
                .Skip((pagina - 1) * RegistrosPorPagina)
                .Take(RegistrosPorPagina)
                .ToListAsync();


            var modeloPaginado = new PaginacionViewModel<Categorias>
            {
                Items = categorias,
                PaginaActual = pagina,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / RegistrosPorPagina)

            };

            return View(modeloPaginado);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] InsertCategoriaDTO insertCategoriaDTO)
        {
            if (ModelState.IsValid)
            {
                var categoria = _mapper.Map<Categorias>(insertCategoriaDTO);

                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();


                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(c => c.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return RedirectToAction("Index"); 
        }
    }
}
