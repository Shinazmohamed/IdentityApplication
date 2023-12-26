using AspNetCoreHero.ToastNotification.Abstractions;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.PermissionHelper;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApplication.Controllers
{
    public class RolesController : BaseController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IRoleBusiness _business;
        private readonly ILogger<RolesController> _logger;
        private readonly INotyfService _notyf;
        public RolesController(IAuthorizationService authorizationService, IRoleBusiness business, ILogger<RolesController> logger, INotyfService notyf)
        {
            _authorizationService = authorizationService;
            _business = business;
            _logger = logger;
            _notyf = notyf;
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
                    _notyf.Success("New role created successfully.");
                }
                else
                {
                    _notyf.Error("Please validate the data.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Controller} All function error", typeof(RolesController));
                _notyf.Error("Error Occured!.");
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
                _notyf.Success("Role deleted successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Error Occured!.");
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
                    _notyf.Success("Role updated successfully.");
                }
                else
                {
                    _notyf.Error("Please validate the data.");
                }
            }
            catch (Exception ex)
            {
                _notyf.Error("Error Occured!.");
                _logger.LogError(ex, "{Controller} All function error", typeof(RolesController));
            }

            return RedirectToAction("Index");
        }
    }
}
