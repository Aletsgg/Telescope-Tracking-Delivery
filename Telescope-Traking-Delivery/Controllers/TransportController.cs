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
    public class TransportController : Controller
    {


        private readonly DB_TRACKER2Context _DBcontext;
        public TransportController(DB_TRACKER2Context context)
        {
            _DBcontext = context;
        }
        //----------------------( INDEX )----------------------
        [Authorize(Roles = "1,2,3")]
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

            List<Transport> lista = _DBcontext.Transports.ToList();
            lista.ForEach(c =>
            {
                c.TransportName = c.TransportName ?? "";
                c.UnitType = c.UnitType ?? "";
                c.Plate = c.Plate ?? "";
                c.Capacity = c.Capacity ?? "";
                c.Details = c.Details ?? "";
                c.UrlImagen = c.UrlImagen ?? "";
                // Continúa para todas las propiedades que quieras convertir
            });

            return View(lista);
        }

        //----------------------( CREAR Y ACTUALIZAR )----------------------
        [Authorize(Roles = "1,2,3")]
        [HttpGet]
        public IActionResult Transport_Detalle(int IdTransport)
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

            Transport Transport = new()
            {

            };
            if (IdTransport != 0)
            {
                Transport = _DBcontext.Transports.Find(IdTransport);
            }
            return View(Transport);
        }

        [Authorize(Roles = "1,2,3")]
        [HttpPost]
        public async Task<IActionResult> Transport_Detalle(Transport Transport, IFormFile Imagen)
        {
            if (Imagen != null)
            {
                //RECIBIR LOS DATOS DEL FORMULARIO
                //Stream image = Imagen.OpenReadStream();

                //GUARDAR LA IMAGEN EN UNA CARPETA
                var path = Path.Combine("C:\\inetpub\\TRACKER\\wwwroot\\images\\Transport", Imagen.FileName);
                // var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Transport", Imagen.FileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await Imagen.CopyToAsync(stream);
                }
                //OBTENER LA DIRECCIÓN DE LA IMAGEN
                var urlImagen = Url.Content($"~/images/Transport/{Imagen.FileName}");

                Transport.UrlImagen = urlImagen;
            }
            

            if (Transport.IdTransport == 0)
            {
                _DBcontext.Transports.Add(Transport);
            }
            else
            {
                _DBcontext.Transports.Update(Transport);
            }
            _DBcontext.SaveChanges();
            return RedirectToAction("Index", "Transport");

        }

        //----------------------( ELIMINAR )----------------------
        [Authorize(Roles = "1,2")]
        [HttpGet]
        public IActionResult Eliminar(int IdTransport)
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



            Transport Transport = _DBcontext.Transports.Where(e => e.IdTransport == IdTransport).FirstOrDefault();

            return View(Transport);


        }
        
        [Authorize(Roles = "1,2")]
        [HttpPost]
        public IActionResult Eliminar(Transport Transport)
        {

            _DBcontext.Transports.Remove(Transport);
            _DBcontext.SaveChanges();

            return RedirectToAction("Index", "Transport");

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

            List<TransportVM> lista = new List<TransportVM>();

            for (int i = 1; i <= cantidadFilas; i++)
            {

                IRow fila = HojaExcel.GetRow(i);

                lista.Add(new TransportVM
                {
                    TransportName = fila.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    UnitType = fila.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Plate = fila.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Capacity = fila.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Details = fila.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Refrigerated = fila.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    EsActivo = fila.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    UrlImagen = fila.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    FechaRegistro = fila.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),



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
            List<Transport> lista = new List<Transport>();

            for (int i = 1; i <= cantidadFilas; i++)
            {

                IRow fila = HojaExcel.GetRow(i);

                lista.Add(new Transport
                {
                    TransportName = fila.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    UnitType = fila.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Plate = fila.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Capacity = fila.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Details = fila.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Refrigerated = bool.Parse(fila.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    EsActivo = bool.Parse(fila.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    UrlImagen = fila.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
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
