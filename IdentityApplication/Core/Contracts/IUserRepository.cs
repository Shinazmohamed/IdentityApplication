using IdentityApplication.Areas.Identity.Data;

namespace IdentityApplication.Core.Contracts
{
    public interface IUserRepository
    {
        ICollection<ApplicationUser> GetUsers();
        ApplicationUser GetUser(string id);
        ApplicationUser UpdateUser(ApplicationUser user);
        ICollection<ApplicationUser> GetUsersWithLocations();
    }
}
