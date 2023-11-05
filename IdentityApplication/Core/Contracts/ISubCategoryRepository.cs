using IdentityApplication.Core.Entities;

namespace IdentityApplication.Core.Contracts
{
    public interface ISubCategoryRepository
    {
        IList<SubCategory> GetSubCategories();
        SubCategory GetSubCategoryById(Guid Id);
        SubCategory GetSubCategoryByName(string Name);
    }
}
