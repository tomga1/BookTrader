﻿using BookTrader.Data;
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

        public async Task<IViewComponentResult> InvokeAsync(bool dropdown = false)
        {
            var categorias = await _context.Categorias.ToListAsync();
            var categoriasOrdenadas = _context.Categorias.OrderBy(c => c.Nombre);
            ViewData["Dropdown"] = dropdown;
            return View(categoriasOrdenadas);
        }

    }
}
