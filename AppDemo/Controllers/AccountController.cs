using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AppDemo.DAL.Interfaces;
using AppDemo.Infrastructure;
using AppDemo.Models;
using AppDemo.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppDemo.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsuarioRepository _usuario;

        public AccountController(IUsuarioRepository usuario)
        {
            _usuario = usuario;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                Usuario user = _usuario.Autenticar(loginViewModel.User, loginViewModel.Password);

                if (user != null)
                {
                    var reclamaciones = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.Nombre),
                    new Claim(ClaimTypes.Role, "administrador")
                };

                    var identidadUsuario = new ClaimsIdentity(reclamaciones, "login");

                    ClaimsPrincipal reclamacionesPrincipales = new ClaimsPrincipal(identidadUsuario);

                    await HttpContext.SignInAsync(Settings.MyCookie, reclamacionesPrincipales);

                    return RedirectToAction("Index", "Archivos");
                }
            }

            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(Settings.MyCookie);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult Unauthorized()
        {
            return View();
        }
    }
}