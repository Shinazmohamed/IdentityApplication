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

        [Authorize(policy: $"{PermissionsModel.Menu.Create}")]
        public async Task<IActionResult> Index()
        {
            var create = await _authorizationService.AuthorizeAsync(User, PermissionsModel.Menu.Create);
            var edit = await _authorizationService.AuthorizeAsync(User, PermissionsModel.Menu.Edit);
            var delete = await _authorizationService.AuthorizeAsync(User, PermissionsModel.Menu.Delete);

            var response = new ManagePermission()
            {
                Create = create.Succeeded,
                Edit = edit.Succeeded,
                Delete = delete.Succeeded
            };

            return View(response);
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.Menu.View}")]
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
    }
}
