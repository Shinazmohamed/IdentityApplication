using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface IMenuBusiness
    {
        Task<List<MenuViewModel>> GetMenus();
    }
}
