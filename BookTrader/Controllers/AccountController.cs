using BookTrader.Models;
using BookTrader.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace BookTrader.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly UserManager<Users> _userManager;

        public AccountController(SignInManager<Users> signInManager , UserManager<Users> userManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;    
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid) 
            {
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

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Users users = new Users
                {
                    NombreCompleto = model.Name,
                    Email = model.Email,
                    UserName = model.Email,
                };

                var result = await _userManager.CreateAsync(users, model.Password);

                if(result.Succeeded)
                {
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
            return View(model);
        }




        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                if(user == null)
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
                if(user != null)
                {
                    var result = await _userManager.RemovePasswordAsync(user);
                    if(result.Succeeded)
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

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
