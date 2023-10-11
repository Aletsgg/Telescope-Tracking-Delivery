using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Telescope_Traking_Delivery.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using EFCore.BulkExtensions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Diagnostics;
using Telescope_Traking_Delivery.Models.ViewModels;

namespace Telescope_Traking_Delivery.Controllers
{
    [Authorize]
    public class CancelacionController : Controller
    {
        private readonly DB_TRACKER2Context _DBcontext;

        public CancelacionController(DB_TRACKER2Context context)
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

        [Authorize(Roles = "1,2,3")]
        public async Task<IActionResult> Index()
        {
            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario;

            int pageSize = 50;
            var cancelacions = _DBcontext.Cancelacions.Include(c => c.IdClientNavigation).Include(t => t.IdOrdersNavigation);
            int pageCount = (int)Math.Ceiling((double)cancelacions.Count() / pageSize);

            List<Cancelacion> lista = new List<Cancelacion>();


            for (int i = 0; i < pageCount; i++)
            {
                var page = await cancelacions.Skip(i * pageSize).Take(pageSize).ToListAsync();
                page.ForEach(p =>
                {
                    p.Entrega = p.Entrega ?? "";
                    p.Factura = p.Factura ?? "";
                    p.Pedido = p.Pedido ?? "";
                    p.Sku = p.Sku ?? "";
                    p.Producto = p.Producto ?? "";
                    p.Lote = p.Lote ?? "";
                    p.Caducidad = p.Caducidad ?? "";
                    p.Motivo = p.Motivo ?? "";
                    p.OriginalResto = p.OriginalResto ?? "";

                });
                lista.AddRange(page);
                // Aquí puedes hacer algo con la página actual de elementos
                // Por ejemplo, podrías agregarlos a la vista
                ViewData["Page" + i] = page;
            }

            return View(lista);
        }





        //----------------------( CREAR Y ACTUALIZAR )----------------------
        [Authorize(Roles = "1,2,3")]
        [HttpGet]
        public IActionResult Cancelacion_Detalle(int IdCancelation)
        {


            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario;
            //---------------------------------------------------------------------




            CancelacionVM oCancelacionVM = new()
            {


                oCancelacion = new Cancelacion(),

                oListaClient = _DBcontext.Clients.Select(Client => new SelectListItem()
                {
                    Text = Client.ClientName,
                    Value = Client.IdClient.ToString()

                }).ToList(),

                oListaOrder = _DBcontext.Orders.Select(Order => new SelectListItem()
                {
                    Text = Order.OrdersDelivery,
                    Value = Order.IdOrders.ToString()

                }).ToList()

            };

            if (IdCancelation != 0)
            {

                oCancelacionVM.oCancelacion = _DBcontext.Cancelacions.Find(IdCancelation);

            }
            return View(oCancelacionVM);
        }


        [Authorize(Roles = "1,2,3")]
        [HttpPost]
        public IActionResult Cancelacion_Detalle(CancelacionVM oCancelacionVM)
        {
            if (oCancelacionVM.oCancelacion.IdCancelation == 0)
            {
                _DBcontext.Cancelacions.Add(oCancelacionVM.oCancelacion);
            }
            else
            {
                _DBcontext.Cancelacions.Update(oCancelacionVM.oCancelacion);
            }
            _DBcontext.SaveChanges();
            return RedirectToAction("Index", "Cancelacion");

        }

        //----------------------( ELIMINAR )----------------------
        [Authorize(Roles = "1,2")]
        [HttpGet]
        public IActionResult Eliminar(int IdCancelation)
        {
            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario;
            //---------------------------------------------------------------------




            Cancelacion oCancelacion = _DBcontext.Cancelacions.Include
                (c => c.IdClientNavigation).Include
                (T => T.IdOrdersNavigation).Where(e => e.IdCancelation == IdCancelation).FirstOrDefault();

            return View(oCancelacion);


        }

        [Authorize(Roles = "1,2")]
        [HttpPost]
        public IActionResult Eliminar(Cancelacion oCancelacion)
        {

            _DBcontext.Cancelacions.Remove(oCancelacion);
            _DBcontext.SaveChanges();

            return RedirectToAction("Index", "Cancelacion");

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

            List<CancelacionVM> lista = new List<CancelacionVM>();

            for (int i = 1; i <= cantidadFilas; i++)
            {

                IRow fila = HojaExcel.GetRow(i);

                lista.Add(new CancelacionVM
                {
                    IdClient = fila.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    IdOrders = fila.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Entrega = fila.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Factura = fila.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Pedido = fila.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Sku = fila.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Producto = fila.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Lote = fila.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Caducidad = fila.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Motivo = fila.GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    OriginalResto = fila.GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    FechaRegistro = fila.GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),



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
            List<Cancelacion> lista = new List<Cancelacion>();

            for (int i = 1; i <= cantidadFilas; i++)
            {

                IRow fila = HojaExcel.GetRow(i);

                lista.Add(new Cancelacion
                {
                    IdClient = int.Parse(fila.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    IdOrders = int.Parse(fila.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    Entrega = fila.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Factura = fila.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Pedido = fila.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Sku = fila.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Producto = fila.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Lote = fila.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Caducidad = fila.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Motivo = fila.GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    OriginalResto = fila.GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    FechaRegistro = DateTime.Parse(fila.GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),                                       

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
