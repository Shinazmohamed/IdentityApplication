using Microsoft.AspNetCore.Identity;

namespace IdentityApplication.Core.Contracts
{
    public interface IRoleRepository
    {
        ICollection<IdentityRole> GetRoles();
        Task Delete(string id);
        Task CreateAsync(IdentityRole role);
        Task<List<IdentityRole>> GetAll();
        Task UpdateAsync(IdentityRole role);
    }
}
