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
        private readonly ILogger<MenuController> _logger;
        public MenuController(IMenuBusiness business, ILogger<MenuController> logger)
        {
            _business = business;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.MenuPermission.View}")]
        public async Task<IActionResult> GetAll([FromBody] PaginationFilter filter)
        {
            var response = new PaginationResponse<MenuViewModel>();
            try
            {
                response = _business.GetMenusWithFilters(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Controller} All function error", typeof(MenuController));
            }
            return Json(new
            {
                filter.draw,
                recordsTotal = response?.TotalCount,
                recordsFiltered = response?.TotalCount,
                data = response?.Data
            });
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.MenuPermission.Edit}")]
        public async Task<IActionResult> Edit(CreateMenuRequest request)
        {
            try
            {
                _business.Update(request);
                TempData["SuccessMessage"] = "Menu updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Menu creation failed.";
                _logger.LogError(ex, "{Controller} All function error", typeof(MenuController));
            }

            return RedirectToAction("Index", "SubMenu");
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.MenuPermission.Delete}")]
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
                _logger.LogError(ex, "{Controller} All function error", typeof(MenuController));
            }

            return RedirectToAction("Index", "SubMenu");
        }
    }
}
