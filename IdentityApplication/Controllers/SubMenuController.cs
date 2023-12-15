using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.PermissionHelper;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Controllers
{
    [Authorize]
    public class SubMenuController : Controller
    {
        private readonly ISubMenuBusiness _business;
        private readonly IMenuBusiness _menuBusiness;

        public SubMenuController(ISubMenuBusiness business, IMenuBusiness menuBusiness)
        {
            _business = business;
            _menuBusiness = menuBusiness;
        }

        [Authorize(policy: $"{PermissionsModel.SubMenuPermission.View}")]
        public IActionResult Index()
        {
            var response = new CreateMenuRequest();
            var menus = _menuBusiness.GetAll();

            response.Menus = menus.Select(menu =>
                new SelectListItem(menu.DisplayName, menu.MenuId.ToString(), false)).ToList();

            return View(response);
        }

        [Authorize(policy: $"{PermissionsModel.SubMenuPermission.Create}")]
        public async Task<IActionResult> Create(CreateMenuRequest request)
        {
            try
            {
                if (request.IsParent)
                {
                    _menuBusiness.Create(request);
                    TempData["SuccessMessage"] = "Menu created successfully.";
                }
                else
                {
                    _business.Create(request);
                    TempData["SuccessMessage"] = "Sub Menu created successfully.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Sub Menu creation failed.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.SubMenuPermission.View}")]
        public async Task<IActionResult> GetAll([FromBody] PaginationFilter filter)
        {
            var data = _business.GetSubMenusWithFilters(filter);

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
        [Authorize(policy: $"{PermissionsModel.SubMenuPermission.Edit}")]
        public ActionResult SaveMenuData([FromBody] ManageMenuViewModel menuData)
        {
            _business.Update(menuData);
            return Json(new { success = true, message = "Menu data saved successfully" });
        }

        [Authorize(policy: $"{PermissionsModel.SubMenuPermission.Edit}")]
        public async Task<IActionResult> Edit(CreateMenuRequest request)
        {
            try
            {
                _business.Edit(request);
                TempData["SuccessMessage"] = "Sub Menu update successfully.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Sub Menu update failed.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.SubMenuPermission.Delete}")]
        public async Task<IActionResult> Delete(string hdnSubMenuId)
        {
            try
            {
                await _business.Delete(hdnSubMenuId);
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
