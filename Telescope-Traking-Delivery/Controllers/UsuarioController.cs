using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using EFCore.BulkExtensions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;
using Telescope_Traking_Delivery.Models;
using Telescope_Traking_Delivery.Models.ViewModels;

namespace Tracker.AplicacionWeb.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly DB_TRACKER2Context _DBcontext;

        public UsuarioController(DB_TRACKER2Context context)
        {
            _DBcontext = context;
        }
        //----------------------(GLOBAL CLAIMS)-----------------------------

        // Helper method to get user information from Claims
        private void GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario)
        {
            ClaimsPrincipal ClaimsName = HttpContext.User;
            nombreUsuario = "";
            EmailUser = "";
            UrlFoto = "";
            ROluser = "";
            IdUsuario = "";

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
        }

        //----------------------( INDEX )----------------------
        [Authorize(Roles = "1,2")]
        public IActionResult Index()
        {
            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario;
            //---------------------------------------------------------------------

            List<Usuario> lista = _DBcontext.Usuarios.Include(r => r.IdRolNavigation).ToList();
            lista.ForEach(c =>
            {
                c.Nombre = c.Nombre ?? "";
                c.Correo = c.Correo ?? "";
                c.Telefono = c.Telefono ?? "";
                c.UrlFoto = c.UrlFoto ?? "";
                c.NombreFoto = c.NombreFoto ?? "";
                c.Clave = c.Clave ?? "";
                // Continúa para todas las propiedades que quieras convertir
            });
            return View(lista);
        }

        //----------------------( CREAR Y ACTUALIZAR )----------------------
        [Authorize(Roles = "1,2")]
        [HttpGet]
        public IActionResult Usuario_Detalle(int IdUsuario)
        {

            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario1);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario1;
            //---------------------------------------------------------------------

            UsuarioVM oUsuarioVM = new()
            {

                oUsuario = new Usuario(),

                oListaRol = _DBcontext.Rols.Select(Rol => new SelectListItem()
                {
                    Text = Rol.Descripcion,
                    Value = Rol.IdRol.ToString()

                }).ToList()

            };
            if (IdUsuario != 0)
            {
                oUsuarioVM.oUsuario = _DBcontext.Usuarios.Find(IdUsuario);
            }
            return View(oUsuarioVM);
        }

        [Authorize(Roles = "1,2")]
        [HttpPost]
        public IActionResult Usuario_Detalle(UsuarioVM oUsuarioVM)
        {
           
            if (oUsuarioVM.oUsuario.IdUsuario == 0)
            {
                _DBcontext.Usuarios.Add(oUsuarioVM.oUsuario);
            }
            else
            {
                _DBcontext.Usuarios.Update(oUsuarioVM.oUsuario);
            }
            _DBcontext.SaveChanges();
            return RedirectToAction("Index", "Usuario");

        }

        //----------------------( ELIMINAR )----------------------
        [Authorize(Roles = "1")]
        [HttpGet]
        public IActionResult Eliminar(int IdUsuario)
        {
            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario1);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario1;
            //---------------------------------------------------------------------

            Usuario oUsuario = _DBcontext.Usuarios.Include(r => r.IdRolNavigation).Where(e => e.IdUsuario == IdUsuario).FirstOrDefault();

            return View(oUsuario);


        }
        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult Eliminar(Usuario oUsuario)
        {

            _DBcontext.Usuarios.Remove(oUsuario);
            _DBcontext.SaveChanges();

            return RedirectToAction("Index", "Usuario");

        }


        //----------------------( EDITTAR PERFIL USUARIO )----------------------
        [Authorize(Roles = "1,2,3,4")]
        [HttpGet]
        public IActionResult Profile(int IdUsuario)
        {

            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario1);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario1;
            //---------------------------------------------------------------------


            UsuarioVM oUsuarioVM = new()
            {
                oUsuario = new Usuario(),

                oListaRol = _DBcontext.Rols.Select(Rol => new SelectListItem()
                {
                    Text = Rol.Descripcion,
                    Value = Rol.IdRol.ToString()

                }).ToList()

            };

            if (IdUsuario != 0)
            {
                oUsuarioVM.oUsuario = _DBcontext.Usuarios.Find(IdUsuario);
            }
            return View(oUsuarioVM);
        }

        [Authorize(Roles = "1,2,3,4")]
        [HttpPost]
        public async Task<IActionResult> Profile(UsuarioVM oUsuarioVM, IFormFile? Imagen)
        {

            if (Imagen != null)
            {
                //RECIBIR LOS DATOS DEL FORMULARIO
                //Stream image = Imagen.OpenReadStream();

                //GUARDAR LA IMAGEN EN UNA CARPETA
               var path = Path.Combine("C:\\inetpub\\TRACKER\\wwwroot\\images\\Usuarios", Imagen.FileName);
                //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Usuarios", Imagen.FileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await Imagen.CopyToAsync(stream);
                }
                //OBTENER LA DIRECCIÓN DE LA IMAGEN
                var urlImagen = Url.Content($"~/images/Usuarios/{Imagen.FileName}");

                oUsuarioVM.oUsuario.UrlFoto = urlImagen;

            }
            else
            {
                ClaimsPrincipal ClaimsName = HttpContext.User;
                string UrlFoto = "";
                if (ClaimsName.Identity.IsAuthenticated)
                {
                    UrlFoto = ClaimsName.Claims.Where(c => c.Type == ClaimTypes.UserData)
                    .Select(c => c.Value).SingleOrDefault();
                }
                oUsuarioVM.oUsuario.UrlFoto = UrlFoto;
            }


            if (oUsuarioVM.oUsuario.IdUsuario == 0)
            {
                _DBcontext.Usuarios.Add(oUsuarioVM.oUsuario);
            }
            else
            {
                _DBcontext.Usuarios.Update(oUsuarioVM.oUsuario);
            }
            _DBcontext.SaveChanges();
            return RedirectToAction("index", "DashBoard");

        }


        //----------------------( CARGAR DATOS EXCEL )----------------------
        [Authorize(Roles = "1")]
        [HttpGet]
        public IActionResult MostrarDatos()
        {
            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario;
            //---------------------------------------------------------------------

            return View();
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult MostrarDatos([FromForm] IFormFile ArchivoExcel)
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

            Stream stream = ArchivoExcel.OpenReadStream();

            IWorkbook MiExcel = null;

            if (Path.GetExtension(ArchivoExcel.FileName) == ".xlsx")
            {
                MiExcel = new XSSFWorkbook(stream);
            }
            else
            {
                MiExcel = new HSSFWorkbook(stream);
            }

            ISheet HojaExcel = MiExcel.GetSheetAt(0);

            int cantidadFilas = HojaExcel.LastRowNum;

            List<UsuarioVM> lista = new List<UsuarioVM>();

            for (int i = 1; i <= cantidadFilas; i++)
            {

                IRow fila = HojaExcel.GetRow(i);

                lista.Add(new UsuarioVM
                {
                    Nombre = fila.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Correo = fila.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Telefono = fila.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    IdRol = fila.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    NombreFoto = fila.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Clave = fila.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    EsActivo = fila.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    FechaRegistro = fila.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),



                });
            }


            return StatusCode(StatusCodes.Status200OK, lista);


        }


        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult EnviarDatos([FromForm] IFormFile ArchivoExcel)
        {
            Stream stream = ArchivoExcel.OpenReadStream();

            IWorkbook MiExcel = null;

            if (Path.GetExtension(ArchivoExcel.FileName) == ".xlsx")
            {
                MiExcel = new XSSFWorkbook(stream);
            }
            else
            {
                MiExcel = new HSSFWorkbook(stream);
            }

            ISheet HojaExcel = MiExcel.GetSheetAt(0);

            int cantidadFilas = HojaExcel.LastRowNum;
            List<Usuario> lista = new List<Usuario>();

            for (int i = 1; i <= cantidadFilas; i++)
            {

                IRow fila = HojaExcel.GetRow(i);

                lista.Add(new Usuario
                {
                    Nombre = fila.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Correo = fila.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Telefono = fila.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    IdRol = int.Parse(fila.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    NombreFoto = fila.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Clave = fila.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    EsActivo = bool.Parse(fila.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    FechaRegistro = DateTime.Parse(fila.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),

                });
            }

            _DBcontext.BulkInsert(lista);

            return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}