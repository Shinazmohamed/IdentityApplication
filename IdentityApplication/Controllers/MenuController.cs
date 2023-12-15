using IdentityApplication.Business;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.PermissionHelper;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApplication.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        private readonly IMenuBusiness _business;
        private readonly IAuthorizationService _authorizationService;
        public MenuController(IMenuBusiness business, IAuthorizationService authorizationService)
        {
            _business = business;
            _authorizationService = authorizationService;
        }

        [Authorize(policy: $"{PermissionsModel.MenuPermission.Create}")]
        public async Task<IActionResult> Index()
        {
            var create = await _authorizationService.AuthorizeAsync(User, PermissionsModel.MenuPermission.Create);
            var edit = await _authorizationService.AuthorizeAsync(User, PermissionsModel.MenuPermission.Edit);
            var delete = await _authorizationService.AuthorizeAsync(User, PermissionsModel.MenuPermission.Delete);

            var response = new ManagePermission()
            {
                Create = create.Succeeded,
                Edit = edit.Succeeded,
                Delete = delete.Succeeded
            };

            return View(response);
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.MenuPermission.View}")]
        public async Task<IActionResult> GetAll([FromBody] PaginationFilter filter)
        {
            var data = _business.GetMenusWithFilters(filter);

            var dataSrc = new
            {
                filter.draw,
                recordsTotal = data.TotalCount,
                recordsFiltered = data.TotalCount,
                data = data.Data
            };
            return Json(dataSrc);
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.MenuPermission.Edit}")]
        public async Task<IActionResult> Edit(CreateMenuRequest request)
        {
            try
            {
                _business.Update(request);
                TempData["SuccessMessage"] = "Menu updated successfully.";

                return RedirectToAction("Index", "SubMenu");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Menu creation failed.";
                return RedirectToAction("Index", "SubMenu");
            }
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.MenuPermission.Delete}")]
        public async Task<IActionResult> Delete(string Id)
        {
            try
            {
                await _business.Delete(Id);
                TempData["SuccessMessage"] = "Record deleted successfully.";

                return RedirectToAction("Index", "SubMenu");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Record delete failed.";
                return RedirectToAction("Index", "SubMenu");
            }
        }
    }
}
