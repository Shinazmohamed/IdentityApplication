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

        public void Create(DepartmentCategory entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (entity == null)
                        throw new ArgumentNullException(nameof(entity));

                    var existingMapping = _context.DepartmentCategories
                        .FirstOrDefault(e => e.DepartmentId == entity.DepartmentId && e.CategoryId == entity.CategoryId);

                    if (existingMapping != null)
                    {
                        throw new ArgumentNullException(nameof(entity));
                    }

                    entity.DepartmentCategoryId = Guid.NewGuid();
                    _context.DepartmentCategories.Add(entity);
                    _context.SaveChanges();

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

        public void Update(DepartmentCategory entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (entity == null)
                        throw new ArgumentNullException(nameof(entity));

                    var existingMapping = _context.DepartmentCategories
                        .FirstOrDefault(e => e.DepartmentCategoryId == entity.DepartmentCategoryId);

                    if (existingMapping == null)
                    {
                        throw new ArgumentNullException(nameof(entity));
                    }

                    existingMapping.DepartmentId = entity.DepartmentId;
                    existingMapping.CategoryId = entity.CategoryId;
                    _context.SaveChanges();

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
                var query = _context.DepartmentCategories
                    .Include(cm => cm.Department)
                    .Include(cm => cm.Category)
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

                var filteredEntities = await query
                    .Skip(filter.start)
                    .Take(filter.length)
                    .ToListAsync();

                var resultViewModel = filteredEntities.Select(entity => new ListCategoryDepartmentMappingViewModel
                {
                    DepartmentCategoryId = entity.DepartmentCategoryId,
                    DepartmentId = entity.DepartmentId,
                    SelectedDepartment = entity.Department.DepartmentName,
                    CategoryId = entity.CategoryId,
                    SelectedCategory = entity.Category.CategoryName
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

        public void Delete(Guid id)
        {
            try
            {

                var entity = _context.DepartmentCategories.FirstOrDefault(e => e.DepartmentCategoryId == id);
                _context.DepartmentCategories.Remove(entity);
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
