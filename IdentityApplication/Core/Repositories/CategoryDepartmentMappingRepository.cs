using AutoMapper;
using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace IdentityApplication.Core.Repositories
{
    public class CategoryDepartmentMappingRepository : ICategoryDepartmentMappingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryDepartmentMappingRepository> _logger;
        private readonly IMapper _mapper;
        public CategoryDepartmentMappingRepository(ApplicationDbContext context, ILogger<CategoryDepartmentMappingRepository> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
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

                    existingMapping.DepartmentId = entity.DepartmentId;
                    _context.SaveChanges();

                    // If everything succeeds, commit the transaction
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(CategoryDepartmentMappingRepository));
                    throw;
                }
            }
        }

        public async Task<PaginationResponse<ListCategoryDepartmentMappingViewModel>> GetEntitiesWithFilters(PaginationFilter filter)
        {
            try
            {
                var query = _context.Category
                    .Include(cm => cm.Department)
                    .OrderBy(e => e.CategoryId)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(filter.department))
                {
                    query = query.Where(e => e.DepartmentId == Guid.Parse(filter.department));
                }

                if (!string.IsNullOrEmpty(filter.category))
                {
                    query = query.Where(e => e.CategoryId == Guid.Parse(filter.category));
                }

                var totalCount = await query.CountAsync();
                var filteredEntities = await query.Where(e => e.DepartmentId != null).Skip(filter.start).Take(filter.length).ToListAsync();

                var resultViewModel = filteredEntities.Select(entity => new ListCategoryDepartmentMappingViewModel
                {
                    DepartmentId = entity.DepartmentId,
                    SelectedDepartment = entity.Department.DepartmentName,
                    CategoryId = entity.CategoryId,
                    SelectedCategory = entity.CategoryName
                }).ToList();

                return new PaginationResponse<ListCategoryDepartmentMappingViewModel>(
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
    }
}
