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

        private readonly ISubMenuRepository _repository;
        public MenuController(IMenuBusiness business, ISubMenuRepository repository)
        {
            _business = business;
            _repository = repository;
        }

        [Authorize(policy: $"{PermissionsModel.Entity.Create}")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.Entity.View}")]
        public async Task<IActionResult> GetAll([FromBody] PaginationFilter filter)
        {
            var data = new IndexViewModel(_business).MenuItems;
            var mappedData = data.Select(entity =>
            {
                var model = entity;
                model.SubMenu = entity.SubMenu;
                return model;
            });

            var dataSrc = new
            {
                filter.draw,
                recordsTotal = data.Count(),
                recordsFiltered = data.Count(),
                data = mappedData
            };
            return Json(dataSrc);
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.Entity.Create}")]
        public ActionResult SaveMenuData([FromBody] ManageMenuViewModel menuData)
        {
            _repository.Update(menuData);
            return Json(new { success = true, message = "Menu data saved successfully" });
        }
    }
}
