using Microsoft.AspNetCore.Mvc;

namespace IdentityApplication.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
