using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityApplication.Core.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MenuRepository> _logger;
        private readonly IMemoryCache _cache;

        public MenuRepository(ApplicationDbContext context, ILogger<MenuRepository> logger, IMemoryCache cache)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }
        public List<Menu> GetMenus()
        {
            try
            {
                const string cacheKey = "Menus";

                if (_cache.TryGetValue(cacheKey, out List<Menu> cachedMenus))
                {
                    return cachedMenus;
                }

                var menus = _context.Menu
                        .OrderBy(e => e.Sort)
                       .Include(e => e.SubMenus)
                       .ToList();

                _cache.Set(cacheKey, menus, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });

                return menus;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(MenuRepository));
                throw;
            }
        }
        public List<Menu> GetMenuById(string roleId)
        {
            try
            {
                var menusWithFilteredSubMenus = _context.Menu
                    .OrderBy(e => e.Sort)
                    .Include(menu => menu.SubMenus)
                        .ThenInclude(subMenu => subMenu.SubMenuRoles.Where(role => role.Id == roleId))
                    .Where(menu => menu.SubMenus.Any(subMenu => subMenu.SubMenuRoles.Any(role => role.Id == roleId)))
                    .ToList();

                foreach (var menu in menusWithFilteredSubMenus)
                {
                    var subMenusToRemove = menu.SubMenus.Where(subMenu => !subMenu.SubMenuRoles.Any()).ToList();

                    foreach (var subMenuToRemove in subMenusToRemove)
                    {
                        menu.SubMenus.Remove(subMenuToRemove);
                    }
                }

                return menusWithFilteredSubMenus;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(MenuRepository));
                throw;
            }
        }

        public void Create(Menu request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    request.Sort = _context.Menu.Count() + 1;
                    _context.Menu.Add(request);
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(MenuRepository));
                    throw;
                }
            }
        }

    }
}
