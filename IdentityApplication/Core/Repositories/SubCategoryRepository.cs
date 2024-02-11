using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;
using Microsoft.EntityFrameworkCore;

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

        public void Create(SubCategory subCategory)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var existingMapping = _context.SubCategory
                        .FirstOrDefault(e => e.SubCategoryName == subCategory.SubCategoryName);

                    if (existingMapping != null)
                    {
                        throw new ArgumentNullException(nameof(existingMapping));
                    }

                    _context.SubCategory.Add(subCategory);
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(SubCategoryRepository));
                }
            }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                var entity = await _context.SubCategory.FirstOrDefaultAsync(e => e.SubCategoryId == id);
                _context.SubCategory.Remove(entity);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(SubCategoryRepository));
            }
        }

        public async Task<PaginationResponse<ListSubCategoryModel>> GetEntitiesWithFilters(PaginationFilter filter)
        {
            try
            {
                var query = _context.SubCategory.OrderBy(e => e.SubCategoryId).AsQueryable();

                if (!string.IsNullOrEmpty(filter.subcategory))
                {
                    query = query.Where(e => e.SubCategoryName == filter.subcategory);
                }

                var totalCount = await query.CountAsync();
                var filteredEntities = await query.Skip(filter.start).Take(filter.length).ToListAsync();

                var resultViewModel = filteredEntities.Select(entity => new ListSubCategoryModel
                {
                    Id = entity.SubCategoryId,
                    Name = entity.SubCategoryName
                }).ToList();

                return new PaginationResponse<ListSubCategoryModel>(
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

        public IList<SubCategory> GetSubCategories()
        {
            var response = new List<SubCategory>();
            try
            {
                response = _context.SubCategory.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(SubCategoryRepository));
            }
            return response;
        }

        public SubCategory GetSubCategoryById(Guid Id)
        {
            var response = new SubCategory();

            try
            {
                response = _context.SubCategory.FirstOrDefault(l => l.SubCategoryId == Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(SubCategoryRepository));
            }
            return response;
        }

        public List<SubCategory> GetSubCategoryByCategoryId(Guid Id)
        {
            var response = new List<SubCategory>();

            try
            {
                return _context.SubCategory
                    .Where(c => c.CategorySubCategories
                        .Any(cd => cd.SubCategoryId == c.SubCategoryId && cd.CategoryId == Id))
                    .ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(SubCategoryRepository));
            }
            return response;
        }

        public SubCategory GetSubCategoryByName(string Name)
        {
            var response = new SubCategory();
            try
            {
                response = _context.SubCategory.FirstOrDefault(l => l.SubCategoryName == Name);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(SubCategoryRepository));
            }
            return response;
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

                    existingMapping.SubCategoryName = entity.SubCategoryName;
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(SubCategoryRepository));
                }
            }
        }
    }
}
