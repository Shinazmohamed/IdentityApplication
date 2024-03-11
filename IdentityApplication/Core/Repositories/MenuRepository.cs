using AutoMapper;
using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityApplication.Core.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MenuRepository> _logger;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public MenuRepository(ApplicationDbContext context, ILogger<MenuRepository> logger, IMemoryCache cache, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
            _configuration = configuration;
            _mapper = mapper;
        }
        public List<Menu> GetMenus()
        {
            var response = new List<Menu>();
            try
            {
                var cacheSettings = _configuration.GetSection("AppSettings:CacheSettings");
                bool enableCache = cacheSettings.GetValue<bool>("EnableCache");
                int cacheDurationMinutes = cacheSettings.GetValue<int>("CacheDurationMinutes");

                const string cacheKey = "Menus";

                if (enableCache && _cache.TryGetValue(cacheKey, out List<Menu> cachedMenus))
                {
                    return cachedMenus;
                }

                response = _context.Menu
                        .OrderBy(e => e.Sort)
                        .Include(e => e.SubMenus)
                        .ToList();

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
                _logger.LogError(e, "{Repo} All function error", typeof(MenuRepository));
            }

            return response;
        }
        public List<Menu> GetMenuById(string roleId)
        {
            var response = new List<Menu>();
            try
            {
                response = _context.Menu
                    .OrderBy(e => e.Sort)
                    .Include(menu => menu.SubMenus)
                        .ThenInclude(subMenu => subMenu.SubMenuRoles.Where(role => role.Id == roleId))
                    .Where(menu => menu.SubMenus.Any(subMenu => subMenu.SubMenuRoles.Any(role => role.Id == roleId)))
                    .ToList();

                foreach (var menu in response)
                {
                    var subMenusToRemove = menu.SubMenus.Where(subMenu => !subMenu.SubMenuRoles.Any()).ToList();

                    foreach (var subMenuToRemove in subMenusToRemove)
                    {
                        menu.SubMenus.Remove(subMenuToRemove);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(MenuRepository));
            }
            return response;
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
                }
            }
        }
        public PaginationResponse<MenuViewModel> GetMenusWithFilters(PaginationFilter filter)
        {
            var response = new PaginationResponse<MenuViewModel>();
            try
            {
                var query = _context.Menu.AsQueryable();

                var totalCount = query.Count();
                var filteredEntities = query
                    .Include(menu => menu.SubMenus)
                    .Skip(filter.start)
                    .Take(filter.length)
                    .Select(menu => new MenuViewModel
                    {
                        Id = menu.MenuId,
                        DisplayName = menu.DisplayName,
                        Sort = (int)menu.Sort,
                        SubMenu = menu.SubMenus.Select(submenu => _mapper.Map<SubMenuViewModel>(submenu)).ToList()
                    })
                    .OrderBy(e => e.DisplayName)
                    .ToList();

                response.Data = filteredEntities;
                response.CurrentPage = filter.draw;
                response.PageSize = filter.length;
                response.TotalCount = totalCount;

                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(MenuRepository));
            }
            return response;
        }
        public void Update(Menu entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (entity == null)
                        throw new ArgumentNullException(nameof(entity));

                    var existingMapping = _context.Menu
                        .FirstOrDefault(e => e.MenuId == entity.MenuId);

                    if (existingMapping == null)
                    {
                        throw new ArgumentNullException(nameof(entity));
                    }

                    existingMapping.DisplayName = entity.DisplayName;
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

        public async Task Delete(Guid id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var entity = await _context.Menu.FirstOrDefaultAsync(e => e.MenuId == id);
                    if(entity !=null)
                        _context.Menu.Remove(entity);

                    var entities = _context.SubMenu.Where(e => e.MenuId == id).ToList();
                    if(entities.Any())
                        _context.SubMenu.RemoveRange(entities);

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
