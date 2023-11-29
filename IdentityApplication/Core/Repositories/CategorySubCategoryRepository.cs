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

        public void Update(SubCategory entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (entity == null)
                        throw new ArgumentNullException(nameof(entity));

                    var existingMapping = _context.SubCategory
                        .FirstOrDefault(e => e.SubCategoryId == entity.SubCategoryId);

                    if (existingMapping == null)
                    {
                        throw new ArgumentNullException(nameof(entity));
                    }

                    existingMapping.CategoryId = entity.CategoryId;
                    _context.SaveChanges();

                    // If everything succeeds, commit the transaction
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "{Repo} All function error", typeof(CategorySubCategoryRepository));

                    // If there's an exception, roll back the transaction
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<PaginationResponse<ListCategorySubCategoryModel>> GetEntitiesWithFilters(PaginationFilter filter)
        {
            try
            {
                var query = _context.SubCategory
                    .Include(cm => cm.Category)
                    .OrderBy(e => e.SubCategoryId)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(filter.category))
                {
                    query = query.Where(e => e.CategoryId == Guid.Parse(filter.category));
                }

                if (!string.IsNullOrEmpty(filter.subcategory))
                {
                    query = query.Where(e => e.SubCategoryName == filter.subcategory);
                }

                var totalCount = await query.CountAsync();
                var filteredEntities = await query.Skip(filter.start).Take(filter.length).ToListAsync();

                var resultViewModel = filteredEntities.Select(entity => new ListCategorySubCategoryModel
                {
                    CategoryId = entity.CategoryId,
                    SelectedCategory = entity.Category.CategoryName,
                    SubCategoryId = entity.SubCategoryId,
                    SelectedSubCategory = entity.SubCategoryName
                    // Add other properties as needed
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
                _logger.LogError(e, "{Repo} All function error", typeof(CategorySubCategoryRepository));
                throw;
            }
        }
    }
}
