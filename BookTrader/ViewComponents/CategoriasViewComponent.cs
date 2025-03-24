using BookTrader.Data;
using BookTrader.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookTrader.ViewComponents
{
    public class CategoriasViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CategoriasViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Obtén la lista de categorías de la base de datos
            IEnumerable<Categorias> categorias = await _context.Categorias.ToListAsync();
            return View(categorias);
        }
    }
}
