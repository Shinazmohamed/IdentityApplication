using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface IAuditBusiness
    {
        Task<PaginationResponse<ListAuditModel>> GetAllWithFilters(PaginationFilter filter);
    }
}
