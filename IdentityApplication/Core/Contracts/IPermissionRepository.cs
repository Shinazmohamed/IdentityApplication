using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Core.Contracts
{
    public interface IPermissionRepository
    {
        Task<PaginationResponse<Entity>> GetEntitiesWithFilters(PaginationFilter filter);
        Task<PaginationResponse<Permission>> GetPermissionsWithFilters(PaginationFilter filter);
        void Create(Entities.Permission request);
        void Update(Entities.Permission request);
        Task Delete(Guid id);
    }
}
