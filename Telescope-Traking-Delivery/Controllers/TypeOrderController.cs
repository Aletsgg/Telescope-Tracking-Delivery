using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Telescope_Traking_Delivery.Models;
using Telescope_Traking_Delivery.Models.ViewModels;

namespace Tracker.AplicacionWeb.Controllers
{
    [Authorize]
    public class TypeOrderController : Controller
    {
        private readonly DB_TRACKER2Context _DBcontext;

        public TypeOrderController(DB_TRACKER2Context context)
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

            List<TypeOrder> lista = _DBcontext.TypeOrders.ToList();
            lista.ForEach(c =>
            {
                c.Clasification = c.Clasification ?? "";
                c.DescriptionOrder = c.DescriptionOrder ?? "";
                c.LocalForaneo = c.LocalForaneo ?? "";
                c.NormalOrFast = c.NormalOrFast ?? "";
                c.Promotion = c.Promotion ?? "";
                // Continúa para todas las propiedades que quieras convertir
            });
            return View(lista);
        }


        //----------------------( CREAR Y ACTUALIZAR )----------------------
        [Authorize(Roles = "1,2,3")]
        [HttpGet]
        public IActionResult TypeOrder_Detalle(int IdTypeOrder)
        {
            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario;
            //---------------------------------------------------------------------


            TypeOrder TypeOrder = new()
            {

            };
            if (IdTypeOrder != 0)
            {
                TypeOrder = _DBcontext.TypeOrders.Find(IdTypeOrder);
            }
            return View(TypeOrder);
        }

        [Authorize(Roles = "1,2")]
        [HttpPost]
        public IActionResult TypeOrder_Detalle(TypeOrder TypeOrder)
        {
            if (TypeOrder.IdTypeOrders == 0)
            {
                _DBcontext.TypeOrders.Add(TypeOrder);
            }
            else
            {
                _DBcontext.TypeOrders.Update(TypeOrder);
            }
            _DBcontext.SaveChanges();
            return RedirectToAction("Index", "TypeOrder");

        }

        //----------------------( ELIMINAR )----------------------
        [Authorize(Roles = "1,2")]
        [HttpGet]
        public IActionResult Eliminar(int IdTypeOrder)
        {

            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario;
            //---------------------------------------------------------------------

            TypeOrder TypeOrder = _DBcontext.TypeOrders.Where(e => e.IdTypeOrders == IdTypeOrder).FirstOrDefault();

            return View(TypeOrder);


        }
        [Authorize(Roles = "1,2")]
        [HttpPost]
        public IActionResult Eliminar(TypeOrder TypeOrder)
        {

            _DBcontext.TypeOrders.Remove(TypeOrder);
            _DBcontext.SaveChanges();

            return RedirectToAction("Index", "TypeOrder");

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

            List<TypeOrderVM> lista = new List<TypeOrderVM>();

            for (int i = 1; i <= cantidadFilas; i++)
            {

                IRow fila = HojaExcel.GetRow(i);

                lista.Add(new TypeOrderVM
                {
                    Clasification = fila.GetCell(0,MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    DescriptionOrder = fila.GetCell(1,MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    LocalForaneo = fila.GetCell(2,MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    NormalOrFast = fila.GetCell(3,MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Promotion = fila.GetCell(4,MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    EsActivo = fila.GetCell(5,MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    FechaRegistro = fila.GetCell(6,MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                  


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
            List<TypeOrder> lista = new List<TypeOrder>();

            for (int i = 1; i <= cantidadFilas; i++)
            {

                IRow fila = HojaExcel.GetRow(i);

                lista.Add(new TypeOrder
                {
                    Clasification = fila.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    DescriptionOrder = fila.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    LocalForaneo = fila.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    NormalOrFast = fila.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Promotion = fila.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    EsActivo = bool.Parse(fila.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    FechaRegistro = DateTime.Parse(fila.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),

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

