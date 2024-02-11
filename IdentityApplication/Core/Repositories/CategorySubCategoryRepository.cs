using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace IdentityApplication.Core.Repositories
{
    public class CategorySubCategoryRepository : ICategorySubCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategorySubCategoryRepository> _logger;
        public CategorySubCategoryRepository(ApplicationDbContext context, ILogger<CategorySubCategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Create(CategorySubCategory entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (entity == null)
                        throw new ArgumentNullException(nameof(entity));

                    var existingMapping = _context.CategorySubCategories
                        .FirstOrDefault(e => e.CategoryId == entity.CategoryId && e.SubCategoryId == entity.SubCategoryId);

                    if (existingMapping != null)
                    {
                        throw new ArgumentNullException(nameof(entity));
                    }

                    entity.CategorySubCategoryId = Guid.NewGuid();
                    _context.CategorySubCategories.Add(entity);
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(CategorySubCategoryRepository));
                    throw;
                }
            }
        }

        public void Update(CategorySubCategory entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (entity == null)
                        throw new ArgumentNullException(nameof(entity));

                    var existingMapping = _context.CategorySubCategories
                        .FirstOrDefault( e => e.CategorySubCategoryId == entity.CategorySubCategoryId);

                    if (existingMapping == null)
                    {
                        throw new ArgumentNullException(nameof(entity));
                    }
                    existingMapping.CategoryId = entity.CategoryId;
                    existingMapping.SubCategoryId = entity.SubCategoryId;

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(CategorySubCategoryRepository));
                    throw;
                }
            }
        }

        public async Task<PaginationResponse<ListCategorySubCategoryModel>> GetEntitiesWithFilters(PaginationFilter filter)
        {
            try
            {
                var query = _context.CategorySubCategories
                    .Include(cm => cm.Category)
                    .Include(cm => cm.SubCategory)
                    .OrderBy(e => e.CategoryId)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(filter.category))
                {
                    query = query.Where(e => e.CategoryId == Guid.Parse(filter.category));
                }

                if (!string.IsNullOrEmpty(filter.subcategory))
                {
                    query = query.Where(e => e.SubCategoryId == Guid.Parse(filter.subcategory));
                }

                var totalCount = await query.CountAsync();

                var filteredEntities = await query
                    .Skip(filter.start)
                    .Take(filter.length)
                    .ToListAsync();

                var resultViewModel = filteredEntities.Select(entity => new ListCategorySubCategoryModel
                {
                    CategorySubCategoryId = entity.CategorySubCategoryId,
                    CategoryId = entity.CategoryId,
                    SelectedCategory = entity.Category.CategoryName,
                    SubCategoryId = entity.SubCategoryId,
                    SelectedSubCategory = entity.SubCategory.SubCategoryName,
                }).ToList();

                return new PaginationResponse<ListCategorySubCategoryModel>(
                    resultViewModel,
                    totalCount,
                    filter.draw,
                    filter.length
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(CategoryDepartmentMappingRepository));
                throw;
            }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                var entity = await _context.CategorySubCategories.FirstOrDefaultAsync(e => e.CategorySubCategoryId == id);
                _context.CategorySubCategories.Remove(entity);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(CategoryDepartmentMappingRepository));
                throw;
            }
        }
    }
}
