using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApplication.Controllers
{
    public class EntityController : Controller
    {
        private readonly IEntityBusiness _business;

        public EntityController(IEntityBusiness business)
        {
            _business = business;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ManagePermission request)
        {
            try
            {
                await _business.Create(request.CreateEntity);

                TempData["SuccessMessage"] = "Entity created successfully.";
                return RedirectToAction("Index", "Permission");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Entity creation failed.";
                return RedirectToAction("Index", "Permission");
            }
        }
    }
}
