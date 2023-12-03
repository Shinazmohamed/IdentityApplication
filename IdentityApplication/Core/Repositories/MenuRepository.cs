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
        public IList<Menu> GetMenus()
        {
            try
            {
                const string cacheKey = "Menus";

                if (_cache.TryGetValue(cacheKey, out IList<Menu> cachedMenus))
                {
                    return cachedMenus;
                }

                var menus = _context.Menu
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
        public IList<Menu> GetMenuById(string roleId)
        {
            try
            {
                var menusWithFilteredSubMenus = _context.Menu
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

    }
}
