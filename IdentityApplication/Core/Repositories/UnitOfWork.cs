using IdentityApplication.Core.Contracts;

namespace IdentityApplication.Core.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository User { get; }
        public IRoleRepository Role { get; }
        public ILocationRepository Location { get; }
        public IEmployeeRepository Employee { get; }
        public IDepartmentRepository Department { get; }
        public ICategoryRepository Category { get; }
        public ISubCategoryRepository SubCategory { get; }
        public UnitOfWork(IUserRepository user, IRoleRepository role, ILocationRepository location, IEmployeeRepository employee, IDepartmentRepository department,
            ICategoryRepository category, ISubCategoryRepository subCategory)
        {
            User = user;
            Role = role;
            Location = location;
            Employee = employee;
            Department = department;
            Category = category;
            SubCategory = subCategory;
        }
    }
}
