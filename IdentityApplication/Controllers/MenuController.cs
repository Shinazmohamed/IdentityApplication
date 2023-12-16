using IdentityApplication.Business.Contracts;
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
        public MenuController(IMenuBusiness business)
        {
            _business = business;
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
