﻿using BookTrader.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookTrader.ViewComponents
{

    public class IdiomasViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public IdiomasViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool dropdown = false)
        {
            var idioma = await _context.Idiomas
                .OrderBy(c => c.Nombre)
                .ToListAsync();
            ViewData["Dropdown"] = dropdown;
            return View(idioma);
        }
    }
}
