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

        [Authorize(policy: $"{PermissionsModel.SubMenu.Create}")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.SubMenu.Create}")]
        public ActionResult SaveMenuData([FromBody] ManageMenuViewModel menuData)
        {
            _repository.Update(menuData);
            return Json(new { success = true, message = "Menu data saved successfully" });
        }
    }
}
