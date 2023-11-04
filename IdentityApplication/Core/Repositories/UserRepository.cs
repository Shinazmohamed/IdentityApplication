using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
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
    }
}
