﻿using Microsoft.AspNetCore.Mvc;

namespace IdentityApplication.Controllers
{
    [ValidateAntiForgeryToken]
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
