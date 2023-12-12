using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Core.Contracts
{
    public interface IUserRepository
    {
        ICollection<ApplicationUser> GetUsers();
        ApplicationUser GetUser(string id);
        ApplicationUser UpdateUser(ApplicationUser user);
        ICollection<ApplicationUser> GetUsersWithLocations();
        ICollection<ListUsersModel> GetUsersWithRoles();
    }
}
