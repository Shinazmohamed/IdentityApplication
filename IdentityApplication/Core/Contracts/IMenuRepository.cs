using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Core.Contracts
{
    public interface IMenuRepository
    {
        List<Menu> GetMenus();
        List<Menu> GetMenuById(string roleId);
        void Create(Menu request);
        PaginationResponse<MenuViewModel> GetMenusWithFilters(PaginationFilter filter);
    }
}
