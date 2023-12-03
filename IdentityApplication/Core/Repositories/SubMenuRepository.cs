using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityApplication.Core.Repositories
{
    public class SubMenuRepository : ISubMenuRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SubMenuRepository> _logger;
        private readonly IMemoryCache _cache;

        public SubMenuRepository(ApplicationDbContext context, ILogger<SubMenuRepository> logger, IMemoryCache cache)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }

        public void Create(SubMenu request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SubMenu.Add(request);
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(SubMenuRepository));
                    throw;
                }
            }
        }
        public List<SubMenu> GetAll()
        {
            try
            {
                const string cacheKey = "SubMenus";

                if (_cache.TryGetValue(cacheKey, out List<SubMenu> cachedSubMenus))
                {
                    return cachedSubMenus;
                }

                var submenus = _context.SubMenu.ToList();

                _cache.Set(cacheKey, submenus, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });

                return submenus;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(SubMenuRepository));
                throw;
            }
        }
    }
}
