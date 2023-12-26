using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.PermissionHelper;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApplication.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IRoleBusiness _business;
        private readonly ILogger<RolesController> _logger;
        public RolesController(IAuthorizationService authorizationService, IRoleBusiness business, ILogger<RolesController> logger)
        {
            _authorizationService = authorizationService;
            _business = business;
            _logger = logger;
        }

        [Authorize(policy: $"{PermissionsModel.RolePermission.View}")]
        public async Task<IActionResult> Index()
        {
            var response = new RolesViewModel();

            try
            {
                var roles = await _business.GetAll();
                var create = await _authorizationService.AuthorizeAsync(User, PermissionsModel.RolePermission.Create);
                var edit = await _authorizationService.AuthorizeAsync(User, PermissionsModel.RolePermission.Edit);
                var delete = await _authorizationService.AuthorizeAsync(User, PermissionsModel.RolePermission.Delete);
                
                response.Roles = roles;
                response.Create = create.Succeeded;
                response.Edit = edit.Succeeded;
                response.Delete = delete.Succeeded;

                var assignPermission = await _authorizationService.AuthorizeAsync(User, PermissionsModel.RolePermission.AssignPermission);
                var assignMenu = await _authorizationService.AuthorizeAsync(User, PermissionsModel.RolePermission.AssignMenu);

                response.AssignMenu = assignMenu.Succeeded;
                response.AssignPermission = assignPermission.Succeeded;
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Controller} All function error", typeof(RolesController));
            }
            return View(response);
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.RolePermission.Create}")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            try
            {
                if (roleName != null)
                {
                    await _business.Create(roleName);
                    TempData["SuccessMessage"] = "Record created successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Record creation failed.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Controller} All function error", typeof(RolesController));
                TempData["ErrorMessage"] = "Record creation failed.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.RolePermission.Delete}")]
        public async Task<IActionResult> Delete(string Id)
        {
            try
            {
                await _business.Delete(Id);
                TempData["SuccessMessage"] = "Record deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Record delete failed.";
                _logger.LogError(ex, "{Controller} All function error", typeof(RolesController));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.RolePermission.Edit}")]
        public async Task<IActionResult> Edit(RolesViewModel role)
        {
            try
            {
                if (role != null)
                {
                    await _business.Update(role);
                    TempData["SuccessMessage"] = "Record updated successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Record update failed.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Controller} All function error", typeof(RolesController));
            }

            return RedirectToAction("Index");
        }
    }
}
