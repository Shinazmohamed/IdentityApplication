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
            //var roleId = "40c2f45d-8a22-499c-b1e9-41c0cf2ad320";

            var res = _context.Menu
                .Include(e => e.SubMenus)
                    .ThenInclude(s => s.SubMenuRoles.Where(i => i.Id == roleId))
                .ToList();

            var ress = _context.Menu
                .Where(menu => menu.SubMenus
                    .Any(subMenu => subMenu.SubMenuRoles
                        .Any(subMenuRole => subMenuRole.Id == roleId)))
                .ToList();


            var itemsToRemove = new List<SubMenu>();

            foreach (var item in ress)
            {
                foreach (var sub in item.SubMenus)
                {
                    if (!sub.SubMenuRoles.Any())
                    {
                        itemsToRemove.Add(sub);
                    }
                }

                // Remove items outside of the inner loop
                foreach (var toRemove in itemsToRemove)
                {
                    item.SubMenus.Remove(toRemove);
                }

                // Clear the list for the next iteration
                itemsToRemove.Clear();
            }

            return ress;
        }
    }
}
