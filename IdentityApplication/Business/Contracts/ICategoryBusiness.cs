using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface ICategoryBusiness
    {
        ListCategoryModel GetCategoryById(string id);
        Task<PaginationResponse<ListCategoryModel>> GetAllWithFilters(PaginationFilter filter);
        Task Create(CreateCategoryRequest request);
        Task Update(CreateCategoryRequest request);
        Task Delete(string id);
    }
}
