using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Telescope_Traking_Delivery.Models;


namespace Telescope_Traking_Delivery.Controllers
{
    public class RegisterController : Controller
    {
        private readonly DB_TRACKER2Context _DBcontext;

        public RegisterController(DB_TRACKER2Context context)
        {
            _DBcontext = context;
        }




        public IActionResult Register()
        {
           
            return View();
        }


        [HttpPost]
        public IActionResult Register(Usuario usuario)
        {
            if (usuario.IdUsuario == 0)
            {
                _DBcontext.Usuarios.Add(usuario);
            }
            else
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            }
            _DBcontext.SaveChanges();

            return RedirectToAction("Login", "Acceso");

        }

    }
}
