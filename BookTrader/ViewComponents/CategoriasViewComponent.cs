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

        public async Task<IViewComponentResult> InvokeAsync(bool dropdown = false, int? selectedId = null)
        {
            var categorias = await _context.Categorias
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            ViewData["Dropdown"] = dropdown;
            ViewData["selectedId"] = selectedId;

            return View(categorias);
        }


    }
}
