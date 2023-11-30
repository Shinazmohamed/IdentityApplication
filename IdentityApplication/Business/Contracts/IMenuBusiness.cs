using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface IMenuBusiness
    {
        List<ViewMenuModel> GetMenus(Guid? roleId);
    }
}
