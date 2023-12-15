using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface ISubMenuBusiness
    {
        List<SubMenu> GetAll();
        void Create(CreateMenuRequest request);
        void Update(ManageMenuViewModel request);
        PaginationResponse<SubMenuViewModel> GetSubMenusWithFilters(PaginationFilter filter);
        void Edit(CreateMenuRequest request);
        Task Delete(string id);
    }
}
