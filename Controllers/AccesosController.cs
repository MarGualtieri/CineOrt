using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BENT1A.Grupo2.Database;
using Reserva_Cine.Models;
using usando_seguridad.Extensions;
using Microsoft.AspNetCore.Authorization;
using BENT1A.Grupo2.Models.Enums;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace BENT1A.Grupo2.Controllers
{
    [AllowAnonymous]
    public class AccesosController : Controller
    {
        private readonly CineDbContext _context;
        private const string _Return_Url = "ReturnUrl";

        public AccesosController(CineDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            // Guardamos la url de retorno para que una vez concluído el login del 
            // usuario lo podamos redirigir a la página en la que se encontraba antes

            TempData[_Return_Url] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password, Rol rol)
        {
            string returnUrl = TempData[_Return_Url] as string;

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                Usuario usuario = null;

            

                if (rol == Rol.Cliente)
                {
                    usuario = _context.Clientes.FirstOrDefault(usr => usr.Email == username);
                }
                else
                {
                    usuario = _context.Empleados.FirstOrDefault(usr => usr.Email == username);
                }

                if (usuario != null)
                {
                    var passwordEncriptada = password.Encriptar();

                    if (usuario.Password.SequenceEqual(passwordEncriptada))
                    {
                        // Se crean las credenciales del usuario que serán incorporadas al contexto
                        ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                        // El lo que luego obtendré al acceder a User.Identity.Name
                        identity.AddClaim(new Claim(ClaimTypes.Name, username));

                        // Se utilizará para la autorización por roles
                        identity.AddClaim(new Claim(ClaimTypes.Role, rol.ToString()));

                        // Lo utilizaremos para acceder al Id del usuario que se encuentra en el sistema.
                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.ID.ToString()));

                        // Lo utilizaremos cuando querramos mostrar el nombre del usuario logueado en el sistema.
                        identity.AddClaim(new Claim(ClaimTypes.GivenName, usuario.Nombre));

                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                        // En este paso se hace el login del usuario al sistema
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).Wait();

                        // Guardo la fecha de último acceso que es ahora.
                        //usuario.FechaUltimoAcceso = DateTime.Now;
                        _context.SaveChanges();

                        TempData["JustLoggedIn"] = true;

                        if (!string.IsNullOrWhiteSpace(returnUrl))
                            return Redirect(returnUrl);

                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
            }

            // Completo estos dos campos para poder retornar a la vista en caso de errores.
            ViewBag.Error = "Usuario o contraseña incorrectos";
            ViewBag.Username = username;
            TempData[_Return_Url] = returnUrl;

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult NoAutorizado()
        {
            return View();
        }
    }
}
