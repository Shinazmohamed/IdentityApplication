using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Core.Contracts
{
    public interface ISubMenuRepository
    {
        void Create(SubMenu request);
        List<SubMenu> GetAll();
        void Update(ManageMenuViewModel request);
        PaginationResponse<SubMenuViewModel> GetSubMenusWithFilters(PaginationFilter filter);
        void Edit(CreateMenuRequest request);
        Task Delete(Guid id);
    }
}
