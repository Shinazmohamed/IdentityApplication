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
        private readonly IConfiguration _configuration;

        public DepartmentRepository(ApplicationDbContext context, IMemoryCache cache, IMapper mapper, ILogger<DepartmentRepository> logger, IConfiguration configuration)
        {
            _context = context;
            _cache = cache;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
        }

        public IList<Department> GetDepartments()
        {
            var response = new List<Department>();
            try
            {
                var cacheSettings = _configuration.GetSection("AppSettings:CacheSettings");
                bool enableCache = cacheSettings.GetValue<bool>("EnableCache");
                int cacheDurationMinutes = cacheSettings.GetValue<int>("CacheDurationMinutes");

                const string cacheKey = "Departments";

                if (enableCache && _cache.TryGetValue(cacheKey, out IList<Department> cachedDepartments))
                {
                    return cachedDepartments;
                }

                response = _context.Department.ToList();
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
                _logger.LogError(e, "{Repo} All function error", typeof(DepartmentRepository));
            }
            return response;
        }
        public Department GetDepartmentById(Guid Id)
        {
            var response = new Department();
            try
            {
                response = _context.Department.Include(e => e.Categories).FirstOrDefault(l => l.DepartmentId == Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(DepartmentRepository));
            }
            return response;
        }
        public Department GetDepartmentByName(string Name)
        {
            var response = new Department();
            try
            {
                response = _context.Department.FirstOrDefault(l => l.DepartmentName == Name);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(DepartmentRepository));
            }
            return response;
        }
        public async Task<PaginationResponse<ListDepartmentViewModel>> GetEntitiesWithFilters(PaginationFilter filter)
        {
            var response = new PaginationResponse<ListDepartmentViewModel>();
            try
            {
                var query = _context.Department.OrderBy(e => e.DepartmentId).AsQueryable();

                if (!string.IsNullOrEmpty(filter.department))
                {
                    query = query.Where(e => e.DepartmentName == filter.department);
                }

                response.TotalCount = await query.CountAsync();
                var filteredEntities = await query.Skip(filter.start).Take(filter.length).ToListAsync();
                response.Data = filteredEntities.Select(entity => _mapper.Map<ListDepartmentViewModel>(entity)).ToList();
                response.CurrentPage = filter.draw;
                response.PageSize = filter.length;

            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(DepartmentRepository));
            }
            return response;
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
                }
            }
        }
    }
}
