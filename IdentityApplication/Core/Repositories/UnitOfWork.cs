using IdentityApplication.Core.Contracts;

namespace IdentityApplication.Core.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository User { get; }
        public IRoleRepository Role { get; }
        public ILocationRepository Location { get; }
        public UnitOfWork(IUserRepository user, IRoleRepository role, ILocationRepository location)
        {
            User = user;
            Role = role;
            Location = location;
        }
    }
}
