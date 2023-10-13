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

namespace Telescope_Traking_Delivery.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly DB_TRACKER2Context _DBcontext;

        public OrderController(DB_TRACKER2Context context)
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
        public async Task<IActionResult> Historial()
        {
            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario;

            int pageSize = 50;
            var orders = _DBcontext.Orders.Include(c => c.IdClientNavigation).Include(t => t.IdTransportNavigation).Include(ts => ts.IdTransportistNavigation).Include(ty => ty.IdTypeOrdersNavigation).Include(u => u.IdUsuarioNavigation);
            int pageCount = (int)Math.Ceiling((double)orders.Count() / pageSize);

            List<Order> lista = new List<Order>();


            for (int i = 0; i < pageCount; i++)
            {
                var page = await orders.Skip(i * pageSize).Take(pageSize).ToListAsync();
                page.ForEach(p =>
                {
                    p.SiteLoading = p.SiteLoading ?? "";
                    p.OrdersDelivery = p.OrdersDelivery ?? "";
                    p.OtOfBoarding = p.OtOfBoarding ?? "";
                    p.OtOfChargingStart = p.OtOfChargingStart ?? "";
                    p.Destination = p.Destination ?? "";
                    p.OnTime = p.OnTime ?? "";
                    p.DeliveryStatus = p.DeliveryStatus ?? "";
                    p.Obsevations = p.Obsevations ?? "";
                    p.ShippingObservations = p.ShippingObservations ?? "";
                    p.IncidentOfArrivalCargoTr = p.IncidentOfArrivalCargoTr ?? "";
                    p.IncidentOfArrivalClientTr = p.IncidentOfArrivalClientTr ?? "";
                    p.IncidentInShipmentWh = p.IncidentInShipmentWh ?? "";
                    p.ExtraRouteIndicator = p.ExtraRouteIndicator ?? "";
                    p.AccidentInRoute = p.AccidentInRoute ?? "";
                    p.RoadTimeIndicators = p.RoadTimeIndicators ?? "";
                });
                lista.AddRange(page);
                // Aquí puedes hacer algo con la página actual de elementos
                // Por ejemplo, podrías agregarlos a la vista
                ViewData["Page" + i] = page;
            }

            return View(lista);

        }

        public IActionResult modal(int IdOrders)
        {
            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario;
            //---------------------------------------------------------------------




            Order oOrder = _DBcontext.Orders.Include
                (c => c.IdClientNavigation).Include
                (T => T.IdTransportNavigation).Include
                (Ts => Ts.IdTransportistNavigation).Include
                (Ty => Ty.IdTypeOrdersNavigation).Where(e => e.IdOrders == IdOrders).FirstOrDefault();

            return View(oOrder);


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
            var orders = _DBcontext.Orders.Include(c => c.IdClientNavigation).Include(t => t.IdTransportNavigation).Include(ts => ts.IdTransportistNavigation).Include(ty => ty.IdTypeOrdersNavigation).Include(u => u.IdUsuarioNavigation);
            int pageCount = (int)Math.Ceiling((double)orders.Count() / pageSize);

            List<Order> lista = new List<Order>();


            for (int i = 0; i < pageCount; i++)
            {
                var page = await orders.Skip(i * pageSize).Take(pageSize).ToListAsync();
                page.ForEach(p =>
                {
                    p.SiteLoading = p.SiteLoading ?? "";
                    p.OrdersDelivery = p.OrdersDelivery ?? "";
                    p.OtOfBoarding = p.OtOfBoarding ?? "";
                    p.OtOfChargingStart = p.OtOfChargingStart ?? "";
                    p.Destination = p.Destination ?? "";
                    p.OnTime = p.OnTime ?? "";
                    p.DeliveryStatus = p.DeliveryStatus ?? "";
                    p.Obsevations = p.Obsevations ?? "";
                    p.ShippingObservations = p.ShippingObservations ?? "";
                    p.IncidentOfArrivalCargoTr = p.IncidentOfArrivalCargoTr ?? "";
                    p.IncidentOfArrivalClientTr = p.IncidentOfArrivalClientTr ?? "";
                    p.IncidentInShipmentWh = p.IncidentInShipmentWh ?? "";
                    p.ExtraRouteIndicator = p.ExtraRouteIndicator ?? "";
                    p.AccidentInRoute = p.AccidentInRoute ?? "";
                    p.RoadTimeIndicators = p.RoadTimeIndicators ?? "";
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
        public IActionResult Order_Detalle(int IdOrders)
        {


            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario;
            //---------------------------------------------------------------------




            OrderVM oOrderVM = new()
            {


                oOrder = new Order(),

                oListaClient = _DBcontext.Clients.Select(Client => new SelectListItem()
                {
                    Text = Client.ClientName,
                    Value = Client.IdClient.ToString()

                

                }).ToList(),

                oListaTransport = _DBcontext.Transports.Select(Transport => new SelectListItem()
                {
                    Text = Transport.Plate,
                    Value = Transport.IdTransport.ToString()

                }).ToList(),

                oListaTransportist = _DBcontext.Transportists.Select(Transportist => new SelectListItem()
                {
                    Text = Transportist.NameTransportist,
                    Value = Transportist.IdTransportist.ToString()

                }).ToList(),


                oListaTypeOrder = _DBcontext.TypeOrders.Select(TypeOrders => new SelectListItem()
                {
                    Text = TypeOrders.DescriptionOrder,
                    Value = TypeOrders.IdTypeOrders.ToString()

                }).ToList(),


                oListaUsuarios = _DBcontext.Usuarios.Select(Usuario => new SelectListItem()
                {
                    Text = Usuario.Nombre,
                    Value = Usuario.IdUsuario.ToString()

                }).ToList()



            };

            if (IdOrders != 0)
            {

                oOrderVM.oOrder = _DBcontext.Orders.Find(IdOrders);

            }

          
      

            return View(oOrderVM);
        }

        
        [Authorize(Roles = "1,2,3")]
        [HttpPost]
        public IActionResult Order_Detalle(OrderVM oOrderVM)
        {
            if (oOrderVM.oOrder.IdOrders == 0)
            {
                _DBcontext.Orders.Add(oOrderVM.oOrder);
            }
            else
            {
                _DBcontext.Orders.Update(oOrderVM.oOrder);
            }
            _DBcontext.SaveChanges();
            return RedirectToAction("Index","Order");

        }

        //----------------------( ELIMINAR )----------------------
        [Authorize(Roles = "1,2")]
        [HttpGet]
        public IActionResult Eliminar(int IdOrders)
        {
            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario;
            //---------------------------------------------------------------------


            Order oOrder = _DBcontext.Orders.Include
                (c => c.IdClientNavigation).Include
                (T => T.IdTransportNavigation).Include
                (Ts => Ts.IdTransportistNavigation).Include
                (Ty => Ty.IdTypeOrdersNavigation).Where(e => e.IdOrders == IdOrders).FirstOrDefault();

            return View(oOrder);


        }
        
        [Authorize(Roles = "1,2")]
        [HttpPost]
        public IActionResult Eliminar(Order oOrder)
        {

            _DBcontext.Orders.Remove(oOrder);
            _DBcontext.SaveChanges();

            return RedirectToAction("Index", "Order");


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

            List<OrderVM> lista = new List<OrderVM>();

            for (int i = 1; i <= cantidadFilas; i++)
            {

                IRow fila = HojaExcel.GetRow(i);

                lista.Add(new OrderVM
                {
                    SiteLoading = fila.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    OrdersDelivery = fila.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    DtAppointedForShipment = fila.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    DtOfBoarding = fila.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    OtOfBoarding = fila.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    VehicleControl = fila.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Destination = fila.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    DtDeliveryAppointment = fila.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Pieces = fila.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    DtArrivalToUnload = fila.GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    OnTime = fila.GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    DeliveryStatus = fila.GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Obsevations = fila.GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    IdClient = fila.GetCell(13, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    IdTypeOrders = fila.GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    IdTransport = fila.GetCell(15, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    IdTransportist = fila.GetCell(16, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Confirmation = fila.GetCell(17, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    EndDateTime = fila.GetCell(18, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    IdUsuario = fila.GetCell(19, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    FechaRegistro = fila.GetCell(20, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),



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
            List<Order> lista = new List<Order>();

            for (int i = 1; i <= cantidadFilas; i++)
            {

                IRow fila = HojaExcel.GetRow(i);

                lista.Add(new Order
                {
                    SiteLoading = fila.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    OrdersDelivery = fila.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    DtAppointedForShipment = DateTime.Parse(fila.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    DtOfBoarding = DateTime.Parse(fila.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    OtOfBoarding = fila.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    VehicleControl = fila.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Destination = fila.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    DtDeliveryAppointment = DateTime.Parse(fila.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    Pieces = int.Parse(fila.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    DtArrivalToUnload = DateTime.Parse(fila.GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    OnTime = fila.GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    DeliveryStatus = fila.GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    Obsevations = fila.GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    IdClient = int.Parse(fila.GetCell(13, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    IdTypeOrders = int.Parse(fila.GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    IdTransport = int.Parse(fila.GetCell(15, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    IdTransportist = int.Parse(fila.GetCell(16, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    Confirmation = DateTime.Parse( fila.GetCell(17, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    EndDateTime = DateTime.Parse(fila.GetCell(18, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    IdUsuario = int.Parse(fila.GetCell(19, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),
                    FechaRegistro = DateTime.Parse( fila.GetCell(20, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()),

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




