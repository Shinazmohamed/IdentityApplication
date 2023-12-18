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
        private readonly ILogger<EntityController> _logger;

        public EntityController(IEntityBusiness business, ILogger<EntityController> logger)
        {
            _business = business;
            _logger = logger;
        }

        [Authorize(policy: $"{PermissionsModel.EntityPermission.View}")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.EntityPermission.Create}")]
        public async Task<IActionResult> Create(ManagePermission request)
        {
            try
            {
                await _business.Create(request);
                TempData["SuccessMessage"] = "Entity created successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Entity creation failed.";
                _logger.LogError(ex, "{Controller} All function error", typeof(EntityController));
            }
            return RedirectToAction("Index", "Permission");
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.EntityPermission.Edit}")]
        public async Task<IActionResult> Edit(ManagePermission request)
        {
            try
            {
                await _business.Edit(request);
                TempData["SuccessMessage"] = "Entity update successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Entity update failed.";
                _logger.LogError(ex, "{Controller} All function error", typeof(EntityController));
            }
            return RedirectToAction("Index", "Permission");
        }

        [Authorize(policy: $"{PermissionsModel.EntityPermission.Delete}")]
        public async Task<IActionResult> Delete(string deleteEntityId)
        {
            try
            {
                await _business.Delete(deleteEntityId);
                TempData["SuccessMessage"] = "Entity deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Entity delete failed.";
                _logger.LogError(ex, "{Controller} All function error", typeof(EntityController));
            }
            return RedirectToAction("Index", "Permission");
        }
    }
}
