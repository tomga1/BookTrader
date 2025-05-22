using BookTrader.Data;
using BookTrader.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookTrader.ViewComponents
{
    public class FormatoViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public FormatoViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool dropdown = false, int selectedId = 0)
        {
            var formato = await _context.Formato
                .OrderBy(c => c.Nombre)
                .ToListAsync();
            ViewData["Dropdown"] = dropdown;
            ViewData["SelectedId"] = selectedId;

            return View(formato);
        }

    }
}
