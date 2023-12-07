using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Controllers
{
    [ValidateAntiForgeryToken]
    public class SubMenuController : Controller
    {
        private readonly ISubMenuBusiness _business;
        private readonly IMenuBusiness _menuBusiness;

        public SubMenuController(ISubMenuBusiness business, IMenuBusiness menuBusiness)
        {
            _business = business;
            _menuBusiness = menuBusiness;
        }

        public IActionResult Index()
        {
            var response = new CreateMenuRequest();
            var menus = _menuBusiness.GetAll();

            response.Menus = menus.Select(menu =>
                new SelectListItem(menu.DisplayName, menu.MenuId.ToString(), false)).ToList();

            return View(response);
        }

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
                TempData["ErrorMessage"] = "Employee creation failed.";
                return RedirectToAction("Index");
            }
        }
    }
}
