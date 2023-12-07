using Microsoft.AspNetCore.Mvc;

namespace IdentityApplication.Controllers
{
    [ValidateAntiForgeryToken]
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
