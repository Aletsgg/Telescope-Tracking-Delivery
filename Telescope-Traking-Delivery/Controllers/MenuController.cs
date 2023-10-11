﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace Telescope_Traking_Delivery.Controllers
{
    [Authorize]

    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
