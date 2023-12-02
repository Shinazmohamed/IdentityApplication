using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityApplication.Core.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MenuRepository> _logger;
        public MenuRepository(ApplicationDbContext context, ILogger<MenuRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IList<Menu> GetMenus()
        {
            return _context.Menu
                .Include(e => e.SubMenus)
                .ToList();
        }
        public IList<Menu> GetMenuById(string roleId)
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

    }
}
