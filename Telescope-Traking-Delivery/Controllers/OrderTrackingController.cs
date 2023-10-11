using Microsoft.AspNetCore.Mvc;

namespace Telescope_Traking_Delivery.Controllers
{
    public class OrderTrackingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
