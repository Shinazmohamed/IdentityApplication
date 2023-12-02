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
            GetMenuById();

            return _context.Menu
                .Include(e => e.SubMenus)
                .ToList();
        }

        public IList<Menu> GetMenuById()
        {
            var res = _context.Menu
                .Include(e => e.SubMenus)
                .ThenInclude(e => e.SubMenuRoles)
                .ToList();
            return res;
        }
    }
}
