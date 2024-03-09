using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Core.Contracts
{
    public interface IStaffRepository
    {
        void Create(Staff request);
        Task<PaginationResponse<Staff>> GetEntitiesWithFilters(PaginationFilter filter);
        void Delete(Guid id);
    }
}
