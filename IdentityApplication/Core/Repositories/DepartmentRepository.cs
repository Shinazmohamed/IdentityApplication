using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;

namespace IdentityApplication.Core.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<Department> GetDepartments()
        {
            return _context.Department.ToList();
        }
        public Department GetDepartmentById(Guid Id)
        {
            return _context.Department.FirstOrDefault(l => l.Id == Id);
        }
        public Department GetDepartmentByName(string Name)
        {
            return _context.Department.FirstOrDefault(l => l.Name == Name);
        }
    }
}
