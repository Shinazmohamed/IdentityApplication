using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityApplication.Core.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RoleRepository> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleRepository(ApplicationDbContext context, ILogger<RoleRepository> logger, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _logger = logger;
            _roleManager = roleManager;
        }

        public ICollection<IdentityRole> GetRoles()
        {
            return _context.Roles.ToList();
        }
        public async Task Delete(string id)
        {
            try
            {
                var entity = await _context.Roles.FirstOrDefaultAsync(e => e.Id == id);
                _context.Roles.Remove(entity);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(RoleRepository));
                throw;
            }
        }
        public async Task CreateAsync(IdentityRole role)
        {
            try
            {
                await _roleManager.CreateAsync(role);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(RoleRepository));
                throw;
            }
        }
        public async Task<List<IdentityRole>> GetAll()
        {
            try
            {
                return await _roleManager.Roles.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(RoleRepository));
                throw;
            }
        }
        public async Task UpdateAsync(IdentityRole role)
        {
            try
            {
                var entity = await _context.Roles.FirstOrDefaultAsync(e => e.Id == role.Id);
                entity.Name = role.Name;
                entity.NormalizedName = role.Name.ToUpper();
                await _roleManager.UpdateAsync(entity);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(RoleRepository));
                throw;
            }
        }
    }
}
