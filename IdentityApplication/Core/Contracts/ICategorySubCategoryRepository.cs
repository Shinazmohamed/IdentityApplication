using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Core.Contracts
{
    public interface ICategorySubCategoryRepository
    {
        void Update(CategorySubCategory entity);
        Task<PaginationResponse<ListCategorySubCategoryModel>> GetEntitiesWithFilters(PaginationFilter filter);
        Task Delete(Guid id);
        void Create(CategorySubCategory entity);
    }
}
