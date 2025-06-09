using BookTrader.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookTrader.ViewComponents
{
    public class CondicionesViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CondicionesViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool dropdown = false, int selectedId = 0)
        {
            var condiciones = await _context.Condiciones
                                           .OrderBy(c => c.Id)
                                           .ToListAsync();

            ViewData["Dropdown"] = dropdown;
            ViewData["SelectedId"] = selectedId;

            return View(condiciones);
        }

    }
}
