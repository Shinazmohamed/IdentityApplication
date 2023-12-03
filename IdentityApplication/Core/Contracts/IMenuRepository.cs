using IdentityApplication.Core.Entities;

namespace IdentityApplication.Core.Contracts
{
    public interface IMenuRepository
    {
        List<Menu> GetMenus();
        List<Menu> GetMenuById(string roleId);
        void Create(Menu request);
    }
}
