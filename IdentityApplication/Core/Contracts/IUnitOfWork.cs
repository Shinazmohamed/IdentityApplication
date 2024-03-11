namespace IdentityApplication.Core.Contracts
{
    public interface IUnitOfWork
    {
        IUserRepository User {  get; }
        IRoleRepository Role { get; }
        ILocationRepository Location { get; }
        IEmployeeRepository Employee { get; }
        IDepartmentRepository Department { get; }
        ICategoryRepository Category { get; }
        ISubCategoryRepository SubCategory { get; }
        ICategorySubCategoryRepository CategorySubCategory { get; }
        IMenuRepository Menu { get; }
        ISubMenuRepository SubMenu { get; }
        IAuditRepository Audit { get; }
        ICategoryDepartmentMappingRepository CategoryDepartmentMapping { get; }
        IPermissionRepository Permission { get; }
        IEntityRepository Entity { get; }
        IPreviousMonthEmployeeRepository PreviousMonthEmployee { get; }
        IStaffRepository Staff { get; }
        ITeamRepository Team { get; }
    }
}
