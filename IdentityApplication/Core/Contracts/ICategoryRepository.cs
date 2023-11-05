using IdentityApplication.Core.Entities;

namespace IdentityApplication.Core.Contracts
{
    public interface ICategoryRepository
    {
        IList<Category> GetCategories();
        Category GetCategoryById(Guid Id);
        Category GetCategoryByName(string Name);
    }
}
