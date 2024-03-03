using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Core.Contracts
{
    public interface IPreviousMonthEmployeeRepository
    {
        Task<PaginationResponse<PreviousMonthEmployee>> GetEntitiesWithFilters(PaginationFilter filter);
        Task<PreviousMonthEmployee> Get(object id);
        Task<PreviousMonthEmployee> Update(Employee entity);
        Task Delete(object id);
        void Create(PreviousMonthEmployee employee);
    }
}
