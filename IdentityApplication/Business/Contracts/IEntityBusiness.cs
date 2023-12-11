using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface IEntityBusiness
    {
        Task Create(CreateEntity request);
        IList<Entity> GetEntities();
    }
}
