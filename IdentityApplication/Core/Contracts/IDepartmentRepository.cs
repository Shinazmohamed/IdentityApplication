using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Core.Contracts
{
    public interface IDepartmentRepository
    {
        IList<Department> GetDepartments();
        Department GetDepartmentById(Guid Id);
        Department GetDepartmentByName(string Name);
        Task<PaginationResponse<ListDepartmentViewModel>> GetEntitiesWithFilters(PaginationFilter filter);
        void Create(Department request);
        void Update(Department request);
        Task Delete(Guid id);
    }
}
