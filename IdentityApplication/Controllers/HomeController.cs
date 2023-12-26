using AspNetCoreHero.ToastNotification.Abstractions;
using IdentityApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IdentityApplication.Controllers
{
    public class HomeController : BaseController
    {
        private readonly INotyfService _notyf;
        public HomeController(INotyfService notyf)
        {
            _notyf = notyf;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}