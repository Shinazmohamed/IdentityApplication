using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface ICategoryDepartmentMappingBusiness
    {
        Task CreateMapping(CreateCategoryDepartmentMappingViewModel request);
        Task UpdateMapping(CreateCategoryDepartmentMappingViewModel request);
        void DeleteMapping(string categoryId);
        Task<PaginationResponse<ListCategoryDepartmentMappingViewModel>> GetAllWithFilters(PaginationFilter filter);
    }
}
