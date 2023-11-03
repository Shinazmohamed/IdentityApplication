namespace IdentityApplication.Core.Contracts
{
    public interface IUnitOfWork
    {
        IUserRepository User {  get; }
        IRoleRepository Role { get; }
    }
}
