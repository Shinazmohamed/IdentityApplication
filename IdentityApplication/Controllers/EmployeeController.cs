using IdentityApplication.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApplication.Controllers
{
    public class EmployeeController : Controller
    {
        [Authorize(Policy = $"{Constants.Policies.RequireAdmin}")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.User}")]
        public IActionResult Administrator()
        {
            return View();
        }
    }
}
