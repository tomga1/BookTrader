using BookTrader.Data;
using BookTrader.Models;
using BookTrader.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Caching.Memory;


namespace BookTrader.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly EmailSender _emailSender;
        private readonly IMemoryCache _memoryCache; 

        public AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, EmailSender emailSender, IMemoryCache memoryCache)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._context = context;
            this._emailSender = emailSender;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    if (user == null)
                    {
                        ModelState.AddModelError("", "El email o la contraseña son incorrectos.");
                        return View(model);
                    }
                    if (!user.EmailConfirmed)
                    {
                        ModelState.AddModelError("", "Debés confirmar tu correo antes de iniciar sesión.");
                        return View(model);
                    }

                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        if (user.IsFirstLogin)
                        {
                            user.IsFirstLogin = false;
                            await _userManager.UpdateAsync(user);
                            return RedirectToAction("MiProfile", "Account");
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "El email o la contraseña son incorrectos.");
                        return View(model);
                    }


                }
                    



                return View(model);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado: " + ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel
            {
                Paises = _context.Paises
                    .OrderBy(p => p.Nombre)
                    .Select(p => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Nombre,
                    }).ToList(),

                // Opcional: si querés dejarlo vacío al principio
                Provincias = new List<SelectListItem>(),
                Localidades = new List<SelectListItem>()

            };

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                // 1. Traer la localidad desde el contexto
                var localidad = await _context.Localidades
                    .Include(l => l.Provincia)
                    .ThenInclude(p => p.Pais)
                    .FirstOrDefaultAsync(l => l.Id == model.LocalidadId.Value);

                if (localidad == null)
                {
                    ModelState.AddModelError("LocalidadId", "La localidad seleccionada no es válida.");
                    return View(model); // Podés reconstruir combos si querés acá
                }
                Users users = new Users
                {
                    NombreCompleto = model.UserName,
                    Email = model.Email,
                    UserName = model.Email,
                    NormalizedUserName = model.Email.ToUpper(),
                    NormalizedEmail = model.Email.ToUpper(),
                    Localidad = localidad // acá le pasás el objeto completo
                };




                var result = await _userManager.CreateAsync(users, model.Password);


                var token = await _userManager.GenerateEmailConfirmationTokenAsync(users);
                var confirmationLink = Url.Action("ConfirmEmail", "Account",
                    new { userId = users.Id, token = token }, Request.Scheme);

                await _emailSender.SendEmailAsync(users.Email, "Confirmá tu cuenta",
                    $"Hacé clic en este enlace para confirmar tu cuenta: <a href='{confirmationLink}'>Confirmar</a>");



                if (result.Succeeded)
                {

                    // Ahora tenés:
                    var nombreLocalidad = localidad.Nombre;
                    var nombreProvincia = localidad.Provincia.Nombre;
                    var nombrePais = localidad.Provincia.Pais.Nombre;


                    var roleExist = await _roleManager.RoleExistsAsync("User");

                    if (!roleExist)
                    {
                        var role = new IdentityRole("User");
                        await _roleManager.CreateAsync(role);

                    }

                    await _userManager.AddToRoleAsync(users, "User");


                    await _signInManager.SignInAsync(users, isPersistent: false);


                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }


                    model.Paises = _context.Paises
                        .Select(p => new SelectListItem
                        {
                            Value = p.Id.ToString(),
                            Text = p.Nombre
                        }).ToList();

                    model.Provincias = _context.Provincias
                        .Where(p => p.PaisId == model.PaisId) // si querés filtrar por el país seleccionado
                        .Select(p => new SelectListItem
                        {
                            Value = p.Id.ToString(),
                            Text = p.Nombre
                        }).ToList();

                    model.Localidades = _context.Localidades
                         .Where(l => l.ProvinciaId == model.ProvinciaId)
                         .Select(l => new SelectListItem
                         {
                             Value = l.Id.ToString(),
                             Text = l.Nombre
                         }).ToList();

                    return View(model);
                }
            }
            return View(model);
        }


        [HttpGet]
        public JsonResult GetProvincias(int paisId)
        {
            string cache_key = $"provincias_{paisId}";

            if(!_memoryCache.TryGetValue(cache_key, out List<SelectListItem> provincias))
            {

                provincias = _context.Provincias
                .Where(p => p.PaisId == paisId)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Nombre
                }).ToList();

                _memoryCache.Set(cache_key,provincias, TimeSpan.FromHours(1));

            }


            return Json(provincias);
        }

        [HttpGet]
        public JsonResult GetLocalidades(int provinciaId)   
        {
            var localidades = _context.Localidades
                .Where(l => l.ProvinciaId == provinciaId)
                .OrderBy(l => l.Nombre)
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Nombre
                }).ToList();

            return Json(localidades);
        }



        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                if (user == null)
                {
                    ViewData["SwalError"] = "Usuario inexistente!";
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangePasswordViewModel { Email = username });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    var result = await _userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email no encontrado!");
                    return View(model);

                }
            }
            else
            {
                ModelState.AddModelError("", "Algo esta mal ,prueba denuevo!");
                return View(model);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View("ConfirmEmail"); // creá esta vista
            }

            return View("Error");
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EmailSender(int id)
        {


            string compradorId = _userManager.GetUserId(User);

            if (compradorId == null)
                return Unauthorized();

            var comprador = await _context.Users
                .Include(u => u.Localidad)
                    .ThenInclude(loc => loc.Provincia)
                        .ThenInclude(prov => prov.Pais)
                .SingleOrDefaultAsync(u => u.Id == compradorId);

            if (comprador == null)
            {
                return Unauthorized();
            }

            // Obtener el libro con el ID
            var libro = await _context.Libros
                .Include(l => l.Publicador)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (libro == null)
                return NotFound();



            var vendedor = libro.Publicador;
            if (vendedor == null)
            {
                return NotFound("El libro no tiene un vendedor asignado");
            }


            var cuerpo = $@"
                            Hola {vendedor.UserName}
                            El usuario <strong>{comprador.NombreCompleto}</strong> está interesado en tu libro <em>{libro.Nombre}</em>.<br><br>
                            <strong>Datos del comprador:</strong><br>
                            Usuario: {comprador.NombreCompleto}<br>
                            Telefono: {comprador.PhoneNumber}<br>
                            Email: {comprador.Email}<br>
                            País: {comprador.Localidad.Provincia.Pais.Nombre}<br>
                            Provincia: {comprador.Localidad.Provincia.Nombre}<br>
                            Localidad: {comprador.Localidad?.Nombre}<br><br>
                            Podés contactarlo para coordinar la entrega.<br><br>
                            ¡Gracias por usar BookTrader!
                            <strong>Importante:</strong><br>
                            Recordá coordinar el encuentro en lugares públicos y seguros. 
                            BookTrader es solo una plataforma de contacto entre usuarios y no se responsabiliza por posibles estafas o inconvenientes durante la transacción. ¡Sé precavido!
                            ";

            await _emailSender.SendEmailAsync(vendedor.Email, "¡Tienen interés en tu libro!", cuerpo);

            TempData["Mensaje"] = "¡El correo fue enviado correctamente al vendedor!";
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> MiProfile()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _context.Users
                .Include(u => u.Localidad)
                    .ThenInclude(l => l.Provincia)
                        .ThenInclude(p => p.Pais)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Localidad == null)
            {
                return NotFound("No se encontró el usuario o su localidad.");
            }

            var paisId = user.Localidad.Provincia.Pais.Id;
            var provinciaId = user.Localidad.Provincia.Id;
            var localidadId = user.Localidad.Id;

            var model = new MiProfileViewModel
            {
                PhoneNumber = user.PhoneNumber,
                PaisId = paisId,
                ProvinciaId = provinciaId,
                LocalidadId = localidadId,

                Paises = _context.Paises
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Nombre
                    }).ToList(),

                Provincias = _context.Provincias
                    .Where(p => p.PaisId == paisId)
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Nombre
                    }).ToList(),

                Localidades = _context.Localidades
                    .Where(l => l.ProvinciaId == provinciaId)
                    .Select(l => new SelectListItem
                    {
                        Value = l.Id.ToString(),
                        Text = l.Nombre
                    }).ToList()
            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MiProfile(MiProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            var result = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar el telefono");
                return View(model); 
            }
            
            await _signInManager.RefreshSignInAsync(user);
            ViewBag.StatusMessage = "Telefono actualizado correctamente.";



            return RedirectToAction("Index", "Home"); 

        }
    }
}
