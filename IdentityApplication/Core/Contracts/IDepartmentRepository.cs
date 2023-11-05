using IdentityApplication.Core.Entities;

namespace IdentityApplication.Core.Contracts
{
    public interface IDepartmentRepository
    {
        IList<Department> GetDepartments();
        Department GetDepartmentById(Guid Id);
        Department GetDepartmentByName(string Name);
    }
}
