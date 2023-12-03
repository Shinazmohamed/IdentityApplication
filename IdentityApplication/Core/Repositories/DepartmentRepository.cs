using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityApplication.Core.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public DepartmentRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
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
    }
}
