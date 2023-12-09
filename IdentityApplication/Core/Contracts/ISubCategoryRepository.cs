using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Core.Contracts
{
    public interface ISubCategoryRepository
    {
        void Create(SubCategory subCategory);
        void Update(SubCategory entity);
        IList<SubCategory> GetSubCategories();
        SubCategory GetSubCategoryById(Guid Id);
        SubCategory GetSubCategoryByName(string Name);
        Task<PaginationResponse<ListSubCategoryModel>> GetEntitiesWithFilters(PaginationFilter filter);
        Task Delete(Guid id);
        List<SubCategory> GetSubCategoryByCategoryId(Guid Id);
    }
}
