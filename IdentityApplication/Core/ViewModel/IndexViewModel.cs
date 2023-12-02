using IdentityApplication.Business.Contracts;

namespace IdentityApplication.Core.ViewModel
{
    public class IndexViewModel
    {
        private readonly IMenuBusiness _business;
        public List<MenuViewModel> MenuItems { get; set; }
        public IndexViewModel(IMenuBusiness business)
        {
            _business = business ?? throw new ArgumentNullException(nameof(business));
            MenuItems = GetMenus().Result;
        }
        private async Task<List<MenuViewModel>> GetMenus()
        {
            return await _business.GetMenus();
        }
    }

}
