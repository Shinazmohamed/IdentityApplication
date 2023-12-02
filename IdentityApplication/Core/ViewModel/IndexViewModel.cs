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
            MenuItems = GetMenus();
        }
        private List<MenuViewModel> GetMenus()
        {
            return _business.GetMenus(Guid.NewGuid());
        }
    }

}
