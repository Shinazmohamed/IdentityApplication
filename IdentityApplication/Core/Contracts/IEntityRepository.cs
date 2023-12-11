using IdentityApplication.Core.Entities;

namespace IdentityApplication.Core.Contracts
{
    public interface IEntityRepository
    {
        IList<Entity> GetEntities();
        void Create(Entity request);
    }
}
