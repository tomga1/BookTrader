using BookTrader.DTOs;
using BookTrader.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BookTrader.Data;
using Microsoft.AspNetCore.Authorization;

namespace BookTrader.Controllers
{
    public class SugerenciasController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context; 

        public SugerenciasController(ApplicationDbContext context ,IMapper mapper)
        {
            _mapper = mapper; 
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] InsertSugerenciaDTO insertSugerenciaDTO)
        {
            if (ModelState.IsValid)
            {
                var sugerencia = _mapper.Map<Sugerencias>(insertSugerenciaDTO);
                _context.Sugerencias.Add(sugerencia);
                await _context.SaveChangesAsync();



                return RedirectToAction("Index", "Home"); // Redirigir al Home/Index
            }

            return RedirectToAction("Index", "Home");
        }


    }
}
