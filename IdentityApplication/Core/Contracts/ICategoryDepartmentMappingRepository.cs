using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Core.Contracts
{
    public interface ICategoryDepartmentMappingRepository
    {
        void Update(Category entity);
        Task<PaginationResponse<ListCategoryDepartmentMappingViewModel>> GetEntitiesWithFilters(PaginationFilter filter);
    }
}
