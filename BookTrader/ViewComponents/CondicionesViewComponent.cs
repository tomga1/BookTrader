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

        public async Task<IViewComponentResult> InvokeAsync(bool dropdown = false)
        {
            var condiciones = await _context.Condiciones.ToListAsync();
            var condicionesOrdenadas = _context.Condiciones.OrderBy(c => c.Nombre);
            ViewData["Dropdown"] = dropdown;
            return View(condicionesOrdenadas);
        }
    }
}
