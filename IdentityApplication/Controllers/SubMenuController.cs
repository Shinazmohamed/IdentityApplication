using AspNetCoreHero.ToastNotification.Abstractions;
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
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<SubMenuController> _logger;
        private readonly INotyfService _notyf;

        public SubMenuController(ISubMenuBusiness business, IMenuBusiness menuBusiness, IAuthorizationService authorizationService, ILogger<SubMenuController> logger, INotyfService notyf)
        {
            _business = business;
            _menuBusiness = menuBusiness;
            _authorizationService = authorizationService;
            _logger = logger;
            _notyf = notyf;
        }

        [Authorize(policy: $"{PermissionsModel.SubMenuPermission.View}")]
        public async Task<IActionResult> Index()
        {
            var response = new CreateMenuRequest();
            var menus = _menuBusiness.GetAll();

            response.Menus = menus.Select(menu =>
                new SelectListItem(menu.DisplayName, menu.MenuId.ToString(), false)).ToList();

            var menu_create = await _authorizationService.AuthorizeAsync(User, PermissionsModel.MenuPermission.Create);
            var menu_edit = await _authorizationService.AuthorizeAsync(User, PermissionsModel.MenuPermission.Edit);
            var menu_delete = await _authorizationService.AuthorizeAsync(User, PermissionsModel.MenuPermission.Delete);
            var menuPermission = new BasePermissionViewModel()
            {
                Create = menu_create.Succeeded,
                Edit = menu_edit.Succeeded,
                Delete = menu_delete.Succeeded,
            };

            var submenu_create = await _authorizationService.AuthorizeAsync(User, PermissionsModel.SubMenuPermission.Create);
            var submenu_edit = await _authorizationService.AuthorizeAsync(User, PermissionsModel.SubMenuPermission.Edit);
            var submenu_delete = await _authorizationService.AuthorizeAsync(User, PermissionsModel.SubMenuPermission.Delete);
            var submenuPermission = new BasePermissionViewModel()
            {
                Create = submenu_create.Succeeded,
                Edit = submenu_edit.Succeeded,
                Delete = submenu_delete.Succeeded,
            };

            response.MenuPermission = menuPermission;
            response.SubMenuPermission = submenuPermission;

            return View(response);
        }

        [Authorize(policy: $"{PermissionsModel.SubMenuPermission.Create}")]
        public async Task<IActionResult> Create(CreateMenuRequest request)
        {
            try
            {
                if (request.IsParent)
                    _menuBusiness.Create(request);
                else
                    _business.Create(request);
                
                _notyf.Success("Record created successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(SubCategoryController));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.SubMenuPermission.View}")]
        public async Task<IActionResult> GetAll([FromBody] PaginationFilter filter)
        {
            var response = new PaginationResponse<SubMenuViewModel>();

            try
            {
                response = _business.GetSubMenusWithFilters(filter);
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(SubCategoryController));
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
        [Authorize(policy: $"{PermissionsModel.SubMenuPermission.Edit}")]
        public ActionResult SaveMenuData([FromBody] ManageMenuViewModel menuData)
        {
            try
            {
                _business.Update(menuData);
                _notyf.Success("Record updated successfully.");

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(SubCategoryController));
            }
            return Json(new { success = false, message = "Menu data saved failed" });
        }

        [Authorize(policy: $"{PermissionsModel.SubMenuPermission.Edit}")]
        public async Task<IActionResult> Edit(CreateMenuRequest request)
        {
            try
            {
                _business.Edit(request);
                _notyf.Success("Record updated successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(SubCategoryController));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.SubMenuPermission.Delete}")]
        public async Task<IActionResult> Delete(string hdnSubMenuId)
        {
            try
            {
                await _business.Delete(hdnSubMenuId);
                _notyf.Success("Record deleted successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(SubCategoryController));
            }
            return RedirectToAction("Index", "SubMenu");
        }
    }
}
