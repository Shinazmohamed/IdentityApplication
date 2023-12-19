using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface IUserBusiness
    {
        Task UpdateUserRole(ApplicationUser user, string selectedRole);
        Task UpdateUser(EditUserViewModel request);
        Task<bool> ResetPassword(EditUserViewModel request, string defaultpassword);
    }
}
