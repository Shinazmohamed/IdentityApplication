using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApplication.ViewComponents
{
    public class ManageMenuViewComponent : ViewComponent
    {
        private readonly IMenuBusiness _business;

        public ManageMenuViewComponent(IMenuBusiness business)
        {
            _business = business;
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
