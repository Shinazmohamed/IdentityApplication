using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IdentityApplication.Core.Repositories
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SubCategoryRepository> _logger;
        public SubCategoryRepository(ApplicationDbContext context, ILogger<SubCategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void CreateMapping(CategoryMapping categoryMapping)
        {
            try
            {
                categoryMapping.Id = Guid.NewGuid();
                _context.CategoryMapping.Add(categoryMapping);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(SubCategoryRepository));
                throw;
            }
        }

        public void UpdateMapping(CategoryMapping entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (entity == null)
                        throw new ArgumentNullException(nameof(entity));

                    var existingMapping = _context.CategoryMapping
                        .FirstOrDefault(e => e.Id == entity.Id);

                    if (existingMapping == null)
                    {
                        throw new ArgumentNullException(nameof(entity));
                    }

                    // Remove the existing entity
                    _context.CategoryMapping.Remove(existingMapping);
                    _context.SaveChanges();

                    // Add the modified entity
                    _context.CategoryMapping.Add(entity);
                    _context.SaveChanges();

                    // If everything succeeds, commit the transaction
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "{Repo} All function error", typeof(SubCategoryRepository));

                    // If there's an exception, roll back the transaction
                    transaction.Rollback();
                    throw;
                }
            }
        }



        public IList<SubCategory> GetSubCategories()
        {
            return _context.SubCategory.ToList();
        }

        public SubCategory GetSubCategoryById(Guid Id)
        {
            return _context.SubCategory.FirstOrDefault(l => l.SubCategoryId == Id);
        }

        public SubCategory GetSubCategoryByName(string Name)
        {
            return _context.SubCategory.FirstOrDefault(l => l.SubCategoryName == Name);
        }

        public async Task<PaginationResponse<ListCategoryMappingModel>> GetEntitiesWithFilters(PaginationFilter filter)
        {
            try
            {
                //var query = _context.CategoryMapping.AsQueryable();

                var query = _context.CategoryMapping
                    .Include(cm => cm.Category)
                    .Include(cm => cm.SubCategory)
                    .AsQueryable();

                var totalCount = await query.CountAsync();
                var filteredEntities = await query.Skip(filter.start).Take(filter.length).ToListAsync();

                var resultViewModel = filteredEntities.Select(entity => new ListCategoryMappingModel
                {
                    Id = entity.Id,
                    CategoryId = entity.CategoryId,
                    SelectedCategory = entity.Category.CategoryName,
                    SubCategoryId = entity.SubCategoryId,
                    SelectedSubCategory = entity.SubCategory.SubCategoryName
                    // Add other properties as needed
                }).ToList();

                return new PaginationResponse<ListCategoryMappingModel>(
                    resultViewModel,
                    totalCount,
                    filter.draw,
                    filter.length
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(SubCategoryRepository));
                throw;
            }
        }

        public async Task DeleteMapping(Guid id)
        {
            try
            {
                var entity = await _context.CategoryMapping.FirstOrDefaultAsync(e => e.Id == id);
                _context.CategoryMapping.Remove(entity);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(SubCategoryRepository));
                throw;
            }
        }
    }
}
