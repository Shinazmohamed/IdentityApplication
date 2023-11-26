using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Core.Contracts
{
    public interface ICategorySubCategoryRepository
    {
        void Update(SubCategory entity);
        Task<PaginationResponse<ListCategorySubCategoryModel>> GetEntitiesWithFilters(PaginationFilter filter);
    }
}
