using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface IPermissionBusiness
    {
        Task<PaginationResponse<Entity>> GetEntitiesWithFilters(PaginationFilter filter);
        Task<PaginationResponse<Permission>> GetPermissionsWithFilters(PaginationFilter filter);
        Task Create(CreatePermission request);
        Task Update(CreatePermission request);
        Task Delete(string id);
    }
}
