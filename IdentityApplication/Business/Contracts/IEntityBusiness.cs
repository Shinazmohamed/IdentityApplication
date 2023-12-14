using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface IEntityBusiness
    {
        Task Create(ManagePermission request);
        IList<Entity> GetEntities();
        Task Edit(ManagePermission request);
        Task Delete(string id);
    }
}
