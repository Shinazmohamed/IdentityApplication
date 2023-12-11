using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Core.Contracts
{
    public interface IPermissionRepository
    {
        Task<PaginationResponse<Entities.Permission>> GetEntitiesWithFilters(PaginationFilter filter);
        void Create(Entities.Permission request);
        void Update(Entities.Permission request);
        Task Delete(Guid id);
    }
}
