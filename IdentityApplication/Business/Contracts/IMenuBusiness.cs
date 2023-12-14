using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface IMenuBusiness
    {
        Task<List<MenuViewModel>> GetMenus();
        void Create(CreateMenuRequest request);
        List<Menu> GetAll();
        PaginationResponse<MenuViewModel> GetMenusWithFilters(PaginationFilter filter);
    }
}
