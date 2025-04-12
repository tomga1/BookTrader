using BookTrader.Data;
using BookTrader.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookTrader.ViewComponents
{
    public class SubCategoriasViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public SubCategoriasViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int categoriaId, bool dropdown)
        {
            var subcategorias = await _context.SubCategorias
                .Where(sc => sc.CategoriaId == categoriaId)
                .ToListAsync();

            System.Diagnostics.Debug.WriteLine($"Se encontraron {subcategorias.Count} subcategorías para la categoría {categoriaId}");


            ViewData["Dropdown"] = dropdown;

            return View(subcategorias); 
        }

    }
}
