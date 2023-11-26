using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface ISubCategoryBusiness
    {
        Task Create(CreateSubCategoryRequest request);
        Task Update(CreateSubCategoryRequest request);
        Task<PaginationResponse<ListSubCategoryModel>> GetAllWithFilters(PaginationFilter filter);
        Task Delete(string id);
        List<ListSubCategoryModel> GetSubCategoriesById(string Id);
    }
}
