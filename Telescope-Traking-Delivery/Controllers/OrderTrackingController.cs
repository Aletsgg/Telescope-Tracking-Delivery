using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Telescope_Traking_Delivery.Models;
using Telescope_Traking_Delivery.Models.viewModels;

namespace Telescope_Traking_Delivery.Controllers
{
    public class OrderTrackingController : Controller
    {
        private readonly DB_TRACKER2Context _DBcontext;

        public OrderTrackingController(DB_TRACKER2Context context)
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



        //----------------------( CREAR Y ACTUALIZAR )----------------------
        [Authorize(Roles = "1,2,3")]
        [HttpGet]
        public IActionResult Segumiento(int IdOrderTracking)
        {


            // Get user information from Claims
            GetUserInfo(out string nombreUsuario, out string EmailUser, out string UrlFoto, out string ROluser, out string IdUsuario);

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["EmailUser"] = EmailUser;
            ViewData["UrlFoto"] = UrlFoto;
            ViewData["ROluser"] = ROluser;
            ViewData["IdUsuario"] = IdUsuario;
            //---------------------------------------------------------------------




            OrderTrackingVM oOrderTrackingVM = new()
            {

                oOrderTracking = new OrderTracking(),

                oListaOrder = _DBcontext.Orders.Select(Order => new SelectListItem()
                {
                    Text = Order.OrdersDelivery,
                    Value = Order.IdOrders.ToString()

                }).ToList()          

            };

            if (IdOrderTracking != 0)
            {

                oOrderTrackingVM.oOrderTracking = _DBcontext.OrderTrackings.Find(IdOrderTracking);

            }




            return View(oOrderTrackingVM);
        }


        [Authorize(Roles = "1,2,3")]
        [HttpPost]
        public IActionResult Segumiento(OrderTrackingVM oOrderTrackingVM)
        {
            if (oOrderTrackingVM.oOrderTracking.IdOrderTracking == 0)
            {
                _DBcontext.OrderTrackings.Add(oOrderTrackingVM.oOrderTracking);
            }
            else
            {
                _DBcontext.OrderTrackings.Update(oOrderTrackingVM.oOrderTracking);
            }
            _DBcontext.SaveChanges();
            return RedirectToAction("Segumiento", "OrderTracking");

        }






        public IActionResult Index()
        {
            return View();
        }
    }
}
