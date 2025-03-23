using BookTrader.Data;
using BookTrader.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BookTrader.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categorias = _context.Categorias.ToList();
            return View(categorias);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string nombre, string descripcion)
        {
            if (ModelState.IsValid)
            {
                var categoria = new Categorias
                {
                    Nombre = nombre,
                    FechaAgregado = DateTime.Now,
                    FechaPublicacion = DateTime.Now,
                    Descripcion = descripcion,
                    IdUsuario = 1
                };

                _context.Add(categoria);
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
