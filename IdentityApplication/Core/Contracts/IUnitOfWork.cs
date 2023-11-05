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
    }
}
