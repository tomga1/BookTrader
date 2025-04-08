using BookTrader.Data;
using BookTrader.Models;
using BookTrader.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace BookTrader.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly EmailSender _emailSender;

        public AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, EmailSender emailSender)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._context = context;
            this._emailSender = emailSender;
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
                    NombreCompleto = model.Name,
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
            var provincias = _context.Provincias
                .Where(p => p.PaisId == paisId)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Nombre
                }).ToList();

            return Json(provincias);
        }

        [HttpGet]
        public JsonResult GetLocalidades(int provinciaId)
        {
            var localidades = _context.Localidades
                .Where(l => l.ProvinciaId == provinciaId)
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
                    ModelState.AddModelError("", "Usuario inexistente!");
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

    }
}
