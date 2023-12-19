using IdentityApplication.Core.Entities;

namespace IdentityApplication.Core.Contracts
{
    public interface IEntityRepository
    {
        List<Entity> GetEntities();
        void Create(Entity request);
        void Edit(Entity entity);
        Task Delete(Guid id);
    }
}
