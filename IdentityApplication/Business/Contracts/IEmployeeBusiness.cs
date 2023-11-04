using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface IEmployeeBusiness
    {
        void Create(Employee request);
        Task<PaginationResponse<Employee>> GetAll(PaginationFilter filter);
        Task<Employee?> GetById(string id);
        void Update(Employee request);
        Task Delete(string id);
    }
}
