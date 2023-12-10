using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Core.Contracts
{
    public interface IPermissionRepository
    {
        Task<PaginationResponse<Entities.Permission>> GetEntitiesWithFilters(PaginationFilter filter);
    }
}
