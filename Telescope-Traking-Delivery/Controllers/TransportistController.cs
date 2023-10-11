using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using EFCore.BulkExtensions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Data;
using Telescope_Traking_Delivery.Models;
using Telescope_Traking_Delivery.Models.ViewModels;

namespace Tracker.AplicacionWeb.Controllers
{
    [Authorize]
    public class TransportistController : Controller
    {
        private readonly DB_TRACKER2Context _DBcontext;
        public TransportistController(DB_TRACKER2Context context)
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
        [Authorize(Roles = "1,2,3")]
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


            List<Transportist> lista = _DBcontext.Transportists.ToList();
            lista.ForEach(c =>
            {
                c.NameTransportist = c.NameTransportist ?? "";
                c.Age = c.Age ?? "";
                c.License = c.License ?? "";
                c.TypeLicense = c.TypeLicense ?? "";
                c.Phone = c.Phone ?? "";
                c.UrlFoto = c.UrlFoto ?? "";
                // Continúa para todas las propiedades que quieras convertir
            });
            return View(lista);
        }

        //----------------------( CREAR Y ACTUALIZAR )----------------------
        [Authorize(Roles = "1,2,3")]
        [HttpGet]
        public IActionResult Transportist_Detalle(int IdTransportist)
        {

            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario;
            //---------------------------------------------------------------------

            Transportist transportist = new()
            {

            };
            if (IdTransportist != 0)
            {
                transportist = _DBcontext.Transportists.Find(IdTransportist);
            }
            return View(transportist);
        }
        
        [Authorize(Roles = "1,2,3")]
        [HttpPost]
        public async Task<IActionResult> Transportist_Detalle(Transportist transportist, IFormFile Imagen)
        {
            if (Imagen != null)
            {
                //RECIBIR LOS DATOS DEL FORMULARIO
                Stream image = Imagen.OpenReadStream();

                //GUARDAR LA IMAGEN EN UNA CARPETA
                var path = Path.Combine("C:\\inetpub\\TRACKER\\wwwroot\\images\\Transportist", Imagen.FileName);
                //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Transportist", Imagen.FileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await Imagen.CopyToAsync(stream);
                }
                //OBTENER LA DIRECCIÓN DE LA IMAGEN
                var urlImagen = Url.Content($"~/images/Transportist/{Imagen.FileName}");

                transportist.UrlFoto = urlImagen;

            }
            if (transportist.IdTransportist == 0)
            {
                _DBcontext.Transportists.Add(transportist);
            }
            else
            {
                _DBcontext.Transportists.Update(transportist);
            }
            _DBcontext.SaveChanges();
            return RedirectToAction("Index", "Transportist");

        }

        //----------------------( ELIMINAR )----------------------
        [Authorize(Roles = "1,2")]
        [HttpGet]
        public IActionResult Eliminar(int IdTransportist)
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

            Transportist transportist = _DBcontext.Transportists.Where(e => e.IdTransportist == IdTransportist).FirstOrDefault();

            return View(transportist);


        }
        
        [Authorize(Roles = "1,2")]
        [HttpPost]
        public IActionResult Eliminar(Transportist transportist)
        {

            _DBcontext.Transportists.Remove(transportist);
            _DBcontext.SaveChanges();

            return RedirectToAction("Index", "Transportist");

        }

        //----------------------( CARGAR DATOS EXCEL )----------------------
        [Authorize(Roles = "1")]
        [HttpGet]
        public IActionResult MostrarDatos()
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
       
        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult MostrarDatos([FromForm] IFormFile ArchivoExcel)
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

            List<TransportistVM> lista = new List<TransportistVM>();

            for (int i = 1; i <= cantidadFilas; i++)
            {

                IRow fila = HojaExcel.GetRow(i);

                lista.Add(new TransportistVM
                {
                    NameTransportist = fila.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Age = fila.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    License = fila.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    TypeLicense = fila.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Phone = fila.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    UrlFoto = fila.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
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
            List<Transportist> lista = new List<Transportist>();

            for (int i = 1; i <= cantidadFilas; i++)
            {

                IRow fila = HojaExcel.GetRow(i);

                lista.Add(new Transportist
                {
                    NameTransportist = fila.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Age = fila.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    License = fila.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    TypeLicense = fila.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Phone = fila.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    UrlFoto = fila.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    EsActivo = bool.Parse(fila.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    FechaRegistro = DateTime.Parse(fila.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),

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