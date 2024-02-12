using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Core.Contracts
{
    public interface IEmployeeRepository
    {
        void Create(Employee employee);
        Task<PaginationResponse<Employee>> GetEntitiesWithFilters(PaginationFilter filter);
        Task<Employee> Get(object id);
        Task<Employee> Update(Employee entity);
        Task Delete(object id);
    }
}
