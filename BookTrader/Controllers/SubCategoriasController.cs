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
    public class SubCategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int RegistrosPorPagina = 10; 
        private readonly IMapper _mapper;

        public SubCategoriasController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper; 
        }

        [HttpGet]
        public IActionResult Lista(int categoriaId)
        {
            return ViewComponent("SubCategorias", new { categoriaId = categoriaId, dropdown = false });
        }
    }
}
