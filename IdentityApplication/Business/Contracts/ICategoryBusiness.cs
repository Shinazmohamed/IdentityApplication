using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface ICategoryBusiness
    {
        ListCategoryModel GetCategoryById(string id);
    }
}
