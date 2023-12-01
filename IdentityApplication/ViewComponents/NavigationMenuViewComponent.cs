using Microsoft.AspNetCore.Mvc;

namespace IdentityApplication.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
