using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using Microsoft.AspNetCore.Identity;

namespace IdentityApplication.Core.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<IdentityRole> GetRoles()
        {
            return _context.Roles.ToList();
        }
    }
}
