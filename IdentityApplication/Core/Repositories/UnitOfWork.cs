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
        public IAuditRepository Audit { get; }
        public ICategoryDepartmentMappingRepository CategoryDepartmentMapping { get; }
        public IPermissionRepository Permission { get; }
        public IEntityRepository Entity { get; }
        public IPreviousMonthEmployeeRepository PreviousMonthEmployee { get; }
        public UnitOfWork(IUserRepository user, IRoleRepository role, ILocationRepository location, IEmployeeRepository employee, IDepartmentRepository department,
            ICategoryRepository category, ICategorySubCategoryRepository categorySubCategory, ISubCategoryRepository subCategory, IMenuRepository menu, ISubMenuRepository subMenu,
            IAuditRepository audit, ICategoryDepartmentMappingRepository categoryDepartmentMapping, IPermissionRepository permission, IEntityRepository entity, IPreviousMonthEmployeeRepository previousMonthEmployee)
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
            Audit = audit;
            CategoryDepartmentMapping = categoryDepartmentMapping;
            Permission = permission;
            Entity = entity;
            PreviousMonthEmployee = previousMonthEmployee;
        }
    }
}
