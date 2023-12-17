using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace IdentityApplication.Business.Contracts
{
    public interface IRoleBusiness
    {
        Task Delete(string id);
        Task Create(string roleName);
        Task<List<IdentityRole>> GetAll();
        Task Update(RolesViewModel role);
    }
}
