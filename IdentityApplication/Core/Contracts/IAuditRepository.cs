using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Core.Contracts
{
    public interface IAuditRepository
    {
        Task<PaginationResponse<ListAuditModel>> GetEntitiesWithFilters(PaginationFilter filter);
    }
}
