using Microsoft.AspNetCore.Identity;

namespace IdentityApplication.Core.Contracts
{
    public interface IRoleRepository
    {
        ICollection<IdentityRole> GetRoles();
    }
}
