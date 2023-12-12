using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace IdentityApplication.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public ICollection<ApplicationUser> GetUsers()
        {
            return _context.Users.ToList();
        }
        public ApplicationUser GetUser(string id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }
        public ApplicationUser UpdateUser(ApplicationUser user)
        {
            _context.Update(user);
            _context.SaveChanges();

            return user;
        }
        public ICollection<ApplicationUser> GetUsersWithLocations()
        {
            return _context.Users
                .Include(e => e.Location)
                .ToList();
        }

        public ICollection<ListUsersModel> GetUsersWithRoless()
        {
            var usersWithRoles = _context.Users
                .Include(e => e.Location)
                .Join(
                    _context.UserRoles,
                    user => user.Id,
                    userRole => userRole.UserId,
                    (user, userRole) => new
                    {
                        User = user,
                        RoleId = userRole.RoleId
                    }
                )
                .Join(
                    _context.Roles,
                    userRole => userRole.RoleId,
                    role => role.Id,
                    (userRole, role) => new
                    {
                        userRole.User,
                        RoleName = role.Name
                    }
                )
                .GroupBy(x => x.User)
                .Select(group => new ListUsersModel
                {
                    Id = Guid.Parse(group.Key.Id),
                    Email = group.Key.Email,
                    LocationName = group.Key.Location.LocationName,
                    Role = group.Select(x => x.RoleName).FirstOrDefault()
                })
                .ToList();

            return usersWithRoles;
        }

        public ICollection<ListUsersModel> GetUsersWithRoles()
        {
            var usersWithRoles = _context.Users
                .Join(
                    _context.UserRoles,
                    user => user.Id,
                    userRole => userRole.UserId,
                    (user, userRole) => new
                    {
                        User = user,
                        RoleId = userRole.RoleId
                    }
                )
                .Join(
                    _context.Roles,
                    userRole => userRole.RoleId,
                    role => role.Id,
                    (userRole, role) => new
                    {
                        userRole.User,
                        RoleName = role.Name
                    }
                )
                .Join(
                    _context.Location,
                    user => user.User.LocationId,
                    location => location.LocationId,
                    (user, location) => new
                    {
                        user.User,
                        Location = location,
                        RoleName = user.RoleName // Add this line to include RoleName in the grouping
                    }
                )
                .GroupBy(x => new { x.User.Id, x.User.Email, x.Location.LocationName, x.RoleName })
                .Select(group => new ListUsersModel
                {
                    Id = Guid.Parse(group.Key.Id),
                    Email = group.Key.Email,
                    LocationName = group.Key.LocationName,
                    Role = group.Key.RoleName // Update this line to use RoleName from the grouping
                })
                .ToList();

            return usersWithRoles;
        }



    }
}
