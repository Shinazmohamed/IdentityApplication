using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.PermissionHelper;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(policy: $"{PermissionsModel.Entity.View}")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.Entity.Create}")]
        public async Task<IActionResult> Create(ManagePermission request)
        {
            try
            {
                await _business.Create(request);

                TempData["SuccessMessage"] = "Entity created successfully.";
                return RedirectToAction("Index", "Permission");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Entity creation failed.";
                return RedirectToAction("Index", "Permission");
            }
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.Entity.Edit}")]
        public async Task<IActionResult> Edit(ManagePermission request)
        {
            try
            {
                await _business.Edit(request);

                TempData["SuccessMessage"] = "Entity update successfully.";
                return RedirectToAction("Index", "Permission");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Entity update failed.";
                return RedirectToAction("Index", "Permission");
            }
        }

        [Authorize(policy: $"{PermissionsModel.Entity.Delete}")]
        public async Task<IActionResult> Delete(string deleteEntityId)
        {
            try
            {
                await _business.Delete(deleteEntityId);
                TempData["SuccessMessage"] = "Entity deleted successfully.";
                return RedirectToAction("Index", "Permission");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Entity delete failed.";
                return RedirectToAction("Index", "Permission");
            }
        }
    }
}
