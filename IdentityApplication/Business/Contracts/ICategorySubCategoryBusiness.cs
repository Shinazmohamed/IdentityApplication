using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface ICategorySubCategoryBusiness
    {
        Task UpdateMapping(CreateCategorySubCategoryRequest request);
        Task CreateMapping(CreateCategorySubCategoryRequest request);
        Task<PaginationResponse<ListCategorySubCategoryModel>> GetAll(PaginationFilter filter);
    }
}
