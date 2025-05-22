using BookTrader.Data;
using BookTrader.DTOs;
using BookTrader.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace BookTrader.Controllers
{
    public class LibrosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int RegistrosPorPagina = 10;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<Users> _userManager;



        public LibrosController(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment, UserManager<Users> userManager)
        {
            _context = context; 
            _mapper = mapper;   
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;       
        }

        [Authorize]
        public async Task<IActionResult> Index(int pagina = 1)
        {
            int totalRegistros = await _context.Libros.CountAsync();
            var libros = await _context.Libros
                .OrderBy(c => c.FechaAgregado) // Ordena por fecha
                .Where(c => c.EstadoPublicacion.Nombre == "Aprobado" && c.EstadoPublicacion.Nombre != "Eliminado por usuario")
                .Skip((pagina - 1) * RegistrosPorPagina)
                .Take(RegistrosPorPagina)
                .ToListAsync();

            

            

            var modeloPaginado = new PaginacionViewModel<Libros>
            {
                Items = libros,
                PaginaActual = pagina,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / RegistrosPorPagina)

            };

            return View(modeloPaginado);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] InsertLibroDTO insertLibroDTO, IFormFile imagenArchivo)
        {

            try
            {
                if (!string.IsNullOrEmpty(insertLibroDTO.ImagenUrl) && imagenArchivo != null)
                {
                    ModelState.AddModelError("", "Solo puedes subir una imagen por URL o por archivo, no ambas.");
                    return View(insertLibroDTO);
                }

                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);

                    string? idUsuario = user?.Id;

                    var libro = _mapper.Map<Libros>(insertLibroDTO);
                    libro.IdUsuario = idUsuario;
                    libro.PublicadorId = idUsuario;

                    // Manejar la imagen

                    if (imagenArchivo != null)
                    {
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imagenArchivo.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imagenArchivo.CopyToAsync(fileStream);
                        }

                        libro.ImagenUrl = "/images/" + uniqueFileName; // Guarda la ruta del archivo en la BD
                    }
                    else
                    {
                        libro.ImagenUrl = insertLibroDTO.ImagenUrl; // Guarda la URL si no subió archivo
                    }

                    _context.Libros.Add(libro);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("MisLibros", "Libros");
                }

                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }


        [Authorize]
        public async Task<IActionResult> MisLibros(int pagina = 1)
        {
            var usuario = await _userManager.GetUserAsync(User);
            string? idUsuario = usuario?.Id;

            int totalRegistros = await _context.Libros
                .Where(l => l.IdUsuario == idUsuario)
                .CountAsync();

            var libros = await _context.Libros
                .Include(l => l.EstadoPublicacion)
                .Where(l => l.IdUsuario == idUsuario && l.EstadoPublicacion.Nombre != "Eliminado por usuario")
                .OrderByDescending(c => c.FechaAgregado)
                .Skip((pagina - 1) * RegistrosPorPagina)
                .Take(RegistrosPorPagina)
                .ToListAsync();

            var modeloPaginado = new PaginacionViewModel<Libros>
            {
                Items = libros,
                PaginaActual = pagina,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistros / RegistrosPorPagina)
            };

            return View(modeloPaginado);
        }


        [Authorize]
        public async Task<IActionResult> Comprar()
        {
            
            return View();
        }


        [Authorize]
        public async Task<IActionResult> Vendido(int id)
        {
            if(id != 0)
            {
                var libro = await _context.Libros.FindAsync(id); 

                

                if(libro != null)
                {


                    var estadoVendido = await _context.EstadoPublicacionEntity.FirstOrDefaultAsync(e => e.Nombre == "Vendido");

                    if (estadoVendido == null) return NotFound("Estado Vendido no encontrado en la base de datos");


                    libro.EstadoPublicacionId = estadoVendido.Id;
                    _context.Libros.Update(libro);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("MisLibros");
                }

                else
                {
                    return NotFound();  
                }

            }

            return BadRequest();    
        }


        
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if (id != 0)
            {
                var libro = await _context.Libros.FindAsync(id);

                if (libro != null)
                {
                    var estadoVendido = await _context.EstadoPublicacionEntity.FirstOrDefaultAsync(e => e.Nombre == "Eliminado por usuario");

                    if (estadoVendido == null) return NotFound("Estado Vendido no encontrado en la base de datos");


                    libro.EstadoPublicacionId = estadoVendido.Id;
                    _context.Libros.Update(libro);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("MisLibros");
                }

                else
                {
                    return NotFound();
                }

            }

            return BadRequest();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var libro = await _context.Libros.FindAsync(id);

            

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Libros model)
        {
            ModelState.Remove("Publicador");
            ModelState.Remove("PublicadorId"); 
            

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var libroExistente = await _context.Libros.FindAsync(model.Id);
            if (libroExistente == null)
            {
                return NotFound();
            }

            // Actualizar campos
            libroExistente.Nombre = model.Nombre;
            libroExistente.Autor = model.Autor;
            libroExistente.Editorial = model.Editorial;
            libroExistente.ISBN = model.ISBN;
            libroExistente.cantPaginas = model.cantPaginas;
            libroExistente.Precio = model.Precio;
            libroExistente.CategoriaId = model.CategoriaId;
            libroExistente.SubCategoriasId = model.SubCategoriasId;
            libroExistente.CondicionId = model.CondicionId;
            libroExistente.FormatoId = model.FormatoId;
            libroExistente.IdiomaId = model.IdiomaId;
            libroExistente.MasInfo = model.MasInfo;

            // Aquí podrías actualizar también la imagen si subió una nueva.

            await _context.SaveChangesAsync();

            return RedirectToAction("MisLibros"); // O a donde quieras ir después de editar
        }


    }
}
