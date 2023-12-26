using AspNetCoreHero.ToastNotification.Abstractions;
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
        private readonly INotyfService _notyf;

        public EntityController(IEntityBusiness business, ILogger<EntityController> logger, INotyfService notyf)
        {
            _business = business;
            _logger = logger;
            _notyf = notyf;
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
                _notyf.Success("Record created successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
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
                _notyf.Success("Record updated successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
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
                _notyf.Success("Record deleted successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(EntityController));
            }
            return RedirectToAction("Index", "Permission");
        }
    }
}
