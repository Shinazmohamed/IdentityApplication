using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityApplication.Core.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryRepository> _logger;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        public CategoryRepository(ApplicationDbContext context, ILogger<CategoryRepository> logger, IMemoryCache cache, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
            _configuration = configuration;
        }

        public IList<Category> GetCategories()
        {
            try
            {
                var cacheSettings = _configuration.GetSection("AppSettings:CacheSettings");
                bool enableCache = cacheSettings.GetValue<bool>("EnableCache");
                int cacheDurationMinutes = cacheSettings.GetValue<int>("CacheDurationMinutes");

                const string cacheKey = "Categories";

                if (enableCache && _cache.TryGetValue(cacheKey, out IList<Category> cachedCategories))
                {
                    return cachedCategories;
                }

                var categories = _context.Category.ToList();

                if (enableCache)
                {
                    _cache.Set(cacheKey, categories, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheDurationMinutes)
                    });
                }

                return categories;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(CategoryRepository));
                throw;
            }
        }

        public Category GetCategoryById(Guid Id)
        {
            try
            {
                return _context.Category.FirstOrDefault(l => l.CategoryId == Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(CategoryRepository));
                throw;
            }
        }

        public List<Category> GetCategoryByDepartmentId(Guid Id)
        {
            try
            {
                return _context.Category
                    .Where(c => c.DepartmentCategories
                        .Any(cd => cd.CategoryId == c.CategoryId && cd.DepartmentId == Id))
                    .ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(CategoryRepository));
                throw;
            }
        }

        public Category GetCategoryByName(string Name)
        {
            try
            {
                return _context.Category.FirstOrDefault(l => l.CategoryName == Name);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(CategoryRepository));
                throw;
            }
        }

        public async Task<PaginationResponse<ListCategoryModel>> GetEntitiesWithFilters(PaginationFilter filter)
        {
            try
            {
                var query = _context.Category.OrderBy(e => e.CategoryId).AsQueryable();

                if (!string.IsNullOrEmpty(filter.category))
                {
                    query = query.Where(e => e.CategoryName == filter.category);
                }

                var totalCount = await query.CountAsync();
                var filteredEntities = await query.Skip(filter.start).Take(filter.length).ToListAsync();

                var resultViewModel = filteredEntities.Select(entity => new ListCategoryModel
                {
                    Id = entity.CategoryId,
                    Name = entity.CategoryName
                }).ToList();

                return new PaginationResponse<ListCategoryModel>(
                    resultViewModel,
                    totalCount,
                    filter.draw,
                    filter.length
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(CategoryRepository));
                throw;
            }
        }
        public void Create(Category request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var existingMapping = _context.Category
                        .FirstOrDefault(e => e.CategoryName == request.CategoryName);

                    if (existingMapping != null)
                    {
                        throw new ArgumentNullException(nameof(existingMapping));
                    }

                    _context.Category.Add(request);
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(CategoryRepository));
                    throw;
                }
            }
        }
        public async Task Delete(Guid id)
        {
            try
            {
                var entity = await _context.Category.FirstOrDefaultAsync(e => e.CategoryId == id);
                _context.Category.Remove(entity);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(CategoryRepository));
                throw;
            }
        }
        public void Update(Category entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (entity == null)
                        throw new ArgumentNullException(nameof(entity));

                    var existingMapping = _context.Category
                        .FirstOrDefault(e => e.CategoryId == entity.CategoryId);

                    if (existingMapping == null)
                    {
                        throw new ArgumentNullException(nameof(entity));
                    }

                    existingMapping.CategoryName = entity.CategoryName;
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(CategoryRepository));
                    throw;
                }
            }
        }
    }
}
