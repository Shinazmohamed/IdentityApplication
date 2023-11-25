using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface ISubCategoryBusiness
    {
        Task CreateMapping(CreateSubCategoryRequest request);
        Task UpdateMapping(CreateSubCategoryRequest request);
        Task<PaginationResponse<ListCategoryMappingModel>> GetAll(PaginationFilter filter);
        Task DeleteMapping(string id);
    }
}
