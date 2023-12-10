using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface IPermissionBusiness
    {
        Task<PaginationResponse<Permission>> GetPermissionsWithFilters(PaginationFilter filter);
    }
}
