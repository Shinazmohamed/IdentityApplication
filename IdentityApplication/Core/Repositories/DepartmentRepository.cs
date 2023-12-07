using AutoMapper;
using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityApplication.Core.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentRepository> _logger;

        public DepartmentRepository(ApplicationDbContext context, IMemoryCache cache, IMapper mapper, ILogger<DepartmentRepository> logger)
        {
            _context = context;
            _cache = cache;
            _mapper = mapper;
            _logger = logger;
        }

        public IList<Department> GetDepartments()
        {
            const string cacheKey = "Departments";

            if (_cache.TryGetValue(cacheKey, out IList<Department> cachedDepartments))
            {
                return cachedDepartments;
            }

            var departments = _context.Department.ToList();

            _cache.Set(cacheKey, departments, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

            return departments;
        }
        public Department GetDepartmentById(Guid Id)
        {
            return _context.Department.FirstOrDefault(l => l.DepartmentId == Id);
        }
        public Department GetDepartmentByName(string Name)
        {
            return _context.Department.FirstOrDefault(l => l.DepartmentName == Name);
        }
        public async Task<PaginationResponse<ListDepartmentViewModel>> GetEntitiesWithFilters(PaginationFilter filter)
        {
            try
            {
                var query = _context.Department.OrderBy(e => e.DepartmentId).AsQueryable();

                if (!string.IsNullOrEmpty(filter.department))
                {
                    query = query.Where(e => e.DepartmentName == filter.department);
                }

                var totalCount = await query.CountAsync();
                var filteredEntities = await query.Skip(filter.start).Take(filter.length).ToListAsync();
                var resultViewModel = filteredEntities.Select(entity => _mapper.Map<ListDepartmentViewModel>(entity)).ToList();

                return new PaginationResponse<ListDepartmentViewModel>(
                    resultViewModel,
                    totalCount,
                    filter.draw,
                    filter.length
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(DepartmentRepository));
                throw;
            }
        }

        public void Create(Department request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var existingMapping = _context.Department
                        .FirstOrDefault(e => e.DepartmentName == request.DepartmentName);

                    if (existingMapping != null)
                    {
                        throw new ArgumentNullException(nameof(existingMapping));
                    }

                    _context.Department.Add(request);
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(DepartmentRepository));
                    throw;
                }
            }
        }
        public async Task Delete(Guid id)
        {
            try
            {
                var entity = await _context.Department.FirstOrDefaultAsync(e => e.DepartmentId == id);
                _context.Department.Remove(entity);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(DepartmentRepository));
                throw;
            }
        }
        public void Update(Department entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (entity == null)
                        throw new ArgumentNullException(nameof(entity));

                    var existingMapping = _context.Department
                        .FirstOrDefault(e => e.DepartmentId == entity.DepartmentId);

                    if (existingMapping == null)
                    {
                        throw new ArgumentNullException(nameof(entity));
                    }

                    existingMapping.DepartmentName = entity.DepartmentName;
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(DepartmentRepository));
                    throw;
                }
            }
        }
    }
}
