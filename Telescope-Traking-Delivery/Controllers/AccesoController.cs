using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Telescope_Traking_Delivery.Logica;
using Telescope_Traking_Delivery.Models;
using Microsoft.AspNetCore.Authorization;
using NPOI.SS.Formula.Functions;

namespace Tracker.AplicacionWeb.Controllers
{
    
    public class AccesoController : Controller
    {

        [HttpGet]
        public IActionResult  Login()
        {
            ClaimsPrincipal Claims = HttpContext.User;
            if (Claims.Identity.IsAuthenticated)
                return RedirectToAction("index", "DashBoard");

            ClaimsPrincipal ClaimsName = HttpContext.User;
            string nombreUsuario = "";

            if (ClaimsName.Identity.IsAuthenticated)
            {
                nombreUsuario = ClaimsName.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();

            }

            ViewData["nombreUsuario"] = nombreUsuario;




            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(string Correo, string Clave, bool CheckMe)
        {
            Usuario objeto = new LO_Usuarios().EncontrarUsuario(Correo, Clave);

            if (objeto.Nombre != null)
            {
                var Claims = new List<Claim>
        {
            new Claim(ClaimTypes.SerialNumber, objeto.IdUsuario.ToString()),
            new Claim(ClaimTypes.Name, objeto.Nombre),
            new Claim(ClaimTypes.Email, objeto.Correo),
            new Claim(ClaimTypes.UserData, objeto.UrlFoto),
            new Claim(ClaimTypes.Role, objeto.IdRol.ToString()),
        };

                var claimsIdentity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Configurar la persistencia de la cookie basada en el valor de CheckMe
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = CheckMe
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("index", "DashBoard");
            }

            return View();
        }


        //Logout

        public async Task<IActionResult> logout()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Acceso");

        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
