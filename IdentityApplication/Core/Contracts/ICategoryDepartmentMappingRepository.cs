using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Core.Contracts
{
    public interface ICategoryDepartmentMappingRepository
    {
        void Update(DepartmentCategory entity);
        Task<PaginationResponse<ListCategoryDepartmentMappingViewModel>> GetEntitiesWithFilters(PaginationFilter filter);
        void Delete(Guid id);
        void Create(DepartmentCategory entity);
    }
}
