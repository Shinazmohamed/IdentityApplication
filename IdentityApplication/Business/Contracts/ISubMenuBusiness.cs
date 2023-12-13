using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface ISubMenuBusiness
    {
        List<SubMenu> GetAll();
        void Create(CreateMenuRequest request);
        PaginationResponse<SubMenuViewModel> GetSubMenusWithFilters(PaginationFilter filter);
    }
}
