using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Telescope_Traking_Delivery.Controllers
{
    [Authorize]
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            ClaimsPrincipal ClaimsName = HttpContext.User;
            string nombreUsuario = "";
            string EmailUser = "";
            string UrlFoto = "";
            string ROluser = "";
            string IdUsuario = "";



            if (ClaimsName.Identity.IsAuthenticated)
            {
                nombreUsuario = ClaimsName.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();


                EmailUser = ClaimsName.Claims.Where(c => c.Type == ClaimTypes.Email)
                    .Select(c => c.Value).SingleOrDefault();

                UrlFoto = ClaimsName.Claims.Where(c => c.Type == ClaimTypes.UserData)
                    .Select(c => c.Value).SingleOrDefault();

                ROluser = ClaimsName.Claims.Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value).SingleOrDefault();

                IdUsuario = ClaimsName.Claims.Where(c => c.Type == ClaimTypes.SerialNumber)
                    .Select(c => c.Value).SingleOrDefault();
            }
            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario;
            //---------------------------------------------------------------------



            return View();
        }
    }
}
