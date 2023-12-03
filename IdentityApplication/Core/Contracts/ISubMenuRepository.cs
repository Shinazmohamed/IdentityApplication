using IdentityApplication.Core.Entities;

namespace IdentityApplication.Core.Contracts
{
    public interface ISubMenuRepository
    {
        void Create(SubMenu request);
        List<SubMenu> GetAll();
    }
}
