using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Core.Contracts
{
    public interface ISubCategoryRepository
    {
        void CreateMapping(CategoryMapping category);
        void UpdateMapping(CategoryMapping entity);
        IList<SubCategory> GetSubCategories();
        SubCategory GetSubCategoryById(Guid Id);
        SubCategory GetSubCategoryByName(string Name);
        Task<PaginationResponse<ListCategoryMappingModel>> GetEntitiesWithFilters(PaginationFilter filter);
        Task DeleteMapping(Guid id);
    }
}
