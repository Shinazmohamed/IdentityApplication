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
        public ICategorySubCategoryRepository CategorySubCategory { get; }
        public ISubCategoryRepository SubCategory { get; }
        public IMenuRepository Menu { get; }
        public ISubMenuRepository SubMenu { get; }
        public UnitOfWork(IUserRepository user, IRoleRepository role, ILocationRepository location, IEmployeeRepository employee, IDepartmentRepository department,
            ICategoryRepository category, ICategorySubCategoryRepository categorySubCategory, ISubCategoryRepository subCategory, IMenuRepository menu, ISubMenuRepository subMenu)
        {
            User = user;
            Role = role;
            Location = location;
            Employee = employee;
            Department = department;
            Category = category;
            CategorySubCategory = categorySubCategory;
            SubCategory = subCategory;
            Menu = menu;
            SubMenu = subMenu;
        }
    }
}
