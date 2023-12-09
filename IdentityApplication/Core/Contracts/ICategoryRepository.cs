using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Core.Contracts
{
    public interface ICategoryRepository
    {
        IList<Category> GetCategories();
        Category GetCategoryById(Guid Id);
        Category GetCategoryByName(string Name);
        Task<PaginationResponse<ListCategoryModel>> GetEntitiesWithFilters(PaginationFilter filter);
        void Create(Category request);
        void Update(Category request);
        Task Delete(Guid id);
        List<Category> GetCategoryByDepartmentId(Guid Id);
    }
}
