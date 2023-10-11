using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Telescope_Traking_Delivery.Controllers
{
    [Authorize]
    public class NegocioController : Controller
    {
        public IActionResult Index()
        {
            ClaimsPrincipal ClaimsName = HttpContext.User;
            string nombreUsuario = "";
            string EmailUser = "";
            string UrlFoto = "";

            if (ClaimsName.Identity.IsAuthenticated)
            {
                nombreUsuario = ClaimsName.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();


                EmailUser = ClaimsName.Claims.Where(c => c.Type == ClaimTypes.Email)
                    .Select(c => c.Value).SingleOrDefault();

                UrlFoto = ClaimsName.Claims.Where(c => c.Type == ClaimTypes.UserData)
                    .Select(c => c.Value).SingleOrDefault();
            }
            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;


            return View();
        }
    }
}
