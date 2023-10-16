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
            return RedirectToAction("Historial", "Order");

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

            return RedirectToAction("Historial", "Order");


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
            // Validación de entrada
            if (ArchivoExcel == null)
            {
                return BadRequest("Archivo no proporcionado");
            }

            // Validación de extensión de archivo
            string extension = Path.GetExtension(ArchivoExcel.FileName);
            if (extension != ".xlsx" && extension != ".xls")
            {
                return BadRequest("Tipo de archivo no soportado");
            }

            try
            {
                Stream stream = ArchivoExcel.OpenReadStream();

                IWorkbook MiExcel = null;

                if (extension == ".xlsx")
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
                        DtOfFarrivalOfUnit = fila.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        OtOfBoarding = fila.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        DtOfChargingStart = fila.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        DtLoadingEnd = fila.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        OtOfChargingStart = fila.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        DtOfDepartureFromSite = fila.GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        VehicleControl = fila.GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        Destination = fila.GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        DtDeliveryAppointment = fila.GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        Pieces = fila.GetCell(13, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        Boxes = fila.GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        DtArrivalToUnload = fila.GetCell(15, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        OnTime = fila.GetCell(16, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        DeliveryStatus = fila.GetCell(17, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        Obsevations = fila.GetCell(18, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        ShippingObservations = fila.GetCell(19, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        IncidentOfArrivalCargoTr = fila.GetCell(20, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        IncidentOfArrivalClientTr = fila.GetCell(21, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        IncidentInShipmentWh = fila.GetCell(22, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        ExtraRouteIndicator = fila.GetCell(23, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        AccidentInRoute = fila.GetCell(24, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        RoadTimeIndicators = fila.GetCell(25, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        IdClient = fila.GetCell(26, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        IdTypeOrders = fila.GetCell(27, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        IdTransport = fila.GetCell(28, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        IdTransportist = fila.GetCell(29, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        Confirmation = fila.GetCell(30, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        EndDateTime = fila.GetCell(31, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        IdUsuario = fila.GetCell(32, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        FechaRegistro = fila.GetCell(33, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),



                    });
                }

                // Cierre de recursos
                stream.Close();

                return StatusCode(StatusCodes.Status200OK, lista);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult EnviarDatos([FromForm] IFormFile ArchivoExcel)
        {
            // Validación de entrada
            if (ArchivoExcel == null)
            {
                return BadRequest("Archivo no proporcionado");
            }

            // Validación de extensión de archivo
            string extension = Path.GetExtension(ArchivoExcel.FileName);
            if (extension != ".xlsx" && extension != ".xls")
            {
                return BadRequest("Tipo de archivo no soportado");
            }

            try
            {
                Stream stream = ArchivoExcel.OpenReadStream();

                IWorkbook MiExcel = null;

                if (extension == ".xlsx")
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

                    // Conversión de fecha
                    DateTime.TryParse(fila.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(), out DateTime DtAppointedForShipment);
                    DateTime.TryParse(fila.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(), out DateTime DtOfBoarding);
                    DateTime.TryParse(fila.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(), out DateTime DtOfFarrivalOfUnit);
                    DateTime.TryParse(fila.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(), out DateTime DtOfChargingStart);
                    DateTime.TryParse(fila.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(), out DateTime DtLoadingEnd);
                    DateTime.TryParse(fila.GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(), out DateTime DtOfDepartureFromSite);
                    DateTime.TryParse(fila.GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(), out DateTime DtDeliveryAppointment);
                    DateTime.TryParse(fila.GetCell(15, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(), out DateTime DtArrivalToUnload);
                    DateTime.TryParse(fila.GetCell(30, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(), out DateTime Confirmation);
                    DateTime.TryParse(fila.GetCell(31, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(), out DateTime EndDateTime);
                    DateTime.TryParse(fila.GetCell(33, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(), out DateTime FechaRegistro);


                    int.TryParse(fila.GetCell(13, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(), out int Pieces);
                    int.TryParse(fila.GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(), out int Boxes);
                    int.TryParse(fila.GetCell(26, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(), out int IdClient);
                    int.TryParse(fila.GetCell(27, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(), out int IdTypeOrders);
                    int.TryParse(fila.GetCell(28, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(), out int IdTransport);
                    int.TryParse(fila.GetCell(29, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(), out int IdTransportist);
                    int.TryParse(fila.GetCell(32, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(), out int IdUsuario);


                    lista.Add(new Order
                    {
                        SiteLoading = fila.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        OrdersDelivery = fila.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        DtAppointedForShipment = DtAppointedForShipment,
                        DtOfBoarding = DtOfBoarding,
                        DtOfFarrivalOfUnit = DtOfFarrivalOfUnit,
                        OtOfBoarding = fila.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        DtOfChargingStart = DtOfChargingStart,
                        DtLoadingEnd = DtLoadingEnd,
                        OtOfChargingStart = fila.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        DtOfDepartureFromSite = DtOfDepartureFromSite,
                        VehicleControl = fila.GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        Destination = fila.GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),                    
                        DtDeliveryAppointment = DtDeliveryAppointment,
                        Pieces = Pieces,
                        Boxes = Boxes,
                        DtArrivalToUnload = DtArrivalToUnload,
                        OnTime = fila.GetCell(16, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        DeliveryStatus = fila.GetCell(17, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        Obsevations = fila.GetCell(18, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        ShippingObservations = fila.GetCell(19, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        IncidentOfArrivalCargoTr = fila.GetCell(20, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        IncidentOfArrivalClientTr = fila.GetCell(21, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        IncidentInShipmentWh = fila.GetCell(22, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        ExtraRouteIndicator = fila.GetCell(23, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        AccidentInRoute = fila.GetCell(24, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        RoadTimeIndicators = fila.GetCell(25, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        IdClient = IdClient, 
                        IdTypeOrders = IdTypeOrders,
                        IdTransport = IdTransport,
                        IdTransportist = IdTransportist,
                        Confirmation = Confirmation,
                        EndDateTime = EndDateTime,
                        IdUsuario = IdUsuario,
                        FechaRegistro = FechaRegistro,

                    });
                }

                // Cierre de recursos
                stream.Close();

                // Inserción masiva
                try
                {
                    _DBcontext.BulkInsert(lista);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}




