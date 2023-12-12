using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityApplication.Core.Repositories
{
    public class SubMenuRepository : ISubMenuRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SubMenuRepository> _logger;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;

        public SubMenuRepository(ApplicationDbContext context, ILogger<SubMenuRepository> logger, IMemoryCache cache, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
            _configuration = configuration;
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
                }
            }
        }
        public List<SubMenu> GetAll()
        {
            var response = new List<SubMenu>();
            try
            {
                var cacheSettings = _configuration.GetSection("AppSettings:CacheSettings");
                bool enableCache = cacheSettings.GetValue<bool>("EnableCache");
                int cacheDurationMinutes = cacheSettings.GetValue<int>("CacheDurationMinutes");

                const string cacheKey = "SubMenus";

                if (enableCache && _cache.TryGetValue(cacheKey, out List<SubMenu> cachedSubMenus))
                {
                    return cachedSubMenus;
                }

                response = _context.SubMenu.ToList();

                if (enableCache)
                {
                    _cache.Set(cacheKey, response, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheDurationMinutes)
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(SubMenuRepository));
            }
            return response;
        }

        public void Delete(ManageMenuViewModel request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var subMenuRolesToRemove = _context.SubMenuRoles.Where(e => e.Id == request.RoleId);
                    _context.SubMenuRoles.RemoveRange(subMenuRolesToRemove);

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} Update function error", typeof(SubMenuRepository));
                }
            }
        }


        public void Update(ManageMenuViewModel request)
        {
            Delete(request);

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in request.menuData.Where(item => item.isChecked))
                    {
                        _context.SubMenuRoles.Add(new SubMenuRole { SubMenuId = item.Id, Id = request.RoleId });
                    }

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} Update function error", typeof(SubMenuRepository));
                }
            }
        }

    }
}
