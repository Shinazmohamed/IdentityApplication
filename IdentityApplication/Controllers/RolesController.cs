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
        public RolesController(IAuthorizationService authorizationService, IRoleBusiness business)
        {
            _authorizationService = authorizationService;
            _business = business;
        }

        [Authorize(policy: $"{PermissionsModel.RolePermission.View}")]
        public async Task<IActionResult> Index()
        {
            var roles = await _business.GetAll();
            var create = await _authorizationService.AuthorizeAsync(User, PermissionsModel.RolePermission.Create);
            var edit = await _authorizationService.AuthorizeAsync(User, PermissionsModel.RolePermission.Edit);
            var delete = await _authorizationService.AuthorizeAsync(User, PermissionsModel.RolePermission.Delete);

            var assignPermission = await _authorizationService.AuthorizeAsync(User, PermissionsModel.RolePermission.AssignPermission);
            var assignMenu = await _authorizationService.AuthorizeAsync(User, PermissionsModel.RolePermission.AssignMenu);

            var response = new RolesViewModel()
            {
                Roles = roles,
                Create = create.Succeeded,
                Edit = edit.Succeeded,
                Delete = delete.Succeeded,
                AssignMenu = assignMenu.Succeeded,
                AssignPermission = assignPermission.Succeeded
            };
            return View(response);
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.RolePermission.Create}")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (roleName != null)
            {
                await _business.Create(roleName);
                TempData["SuccessMessage"] = "Record deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Record delete failed.";
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

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Record delete failed.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.RolePermission.Edit}")]
        public async Task<IActionResult> Edit(RolesViewModel role)
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

            return RedirectToAction("Index");
        }
    }
}
