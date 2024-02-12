using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace IdentityApplication.Core.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeeRepository> _logger;
        public EmployeeRepository(ApplicationDbContext context, ILogger<EmployeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Create(Employee employee)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Employee.Add(employee);
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(EmployeeRepository));
                    throw;
                }
            }
        }
        public async Task<PaginationResponse<Employee>> GetEntitiesWithFilters(PaginationFilter filter)
        {
            try
            {
                var query = _context.Employee.AsNoTracking(); 

                // Apply filters
                if (!string.IsNullOrEmpty(filter.location))
                    query = query.Where(e => e.LocationName == filter.location);

                if (!string.IsNullOrEmpty(filter.department))
                    query = query.Where(e => e.DepartmentName == filter.department);

                if (!string.IsNullOrEmpty(filter.category))
                    query = query.Where(e => e.CategoryName == filter.category);

                if (!string.IsNullOrEmpty(filter.subcategory))
                    query = query.Where(e => e.SubCategoryName == filter.subcategory);

                // Total count
                var totalCount = await query.CountAsync();

                // Order and pagination
                query = query.OrderBy(e => e.LocationName)
                             .ThenBy(e => e.DepartmentName)
                             .ThenBy(e => e.CategoryName)
                             .ThenBy(e => e.SubCategoryName)
                             .Skip(filter.start)
                             .Take(filter.length);

                var filteredEntities = await query.ToListAsync();

                return new PaginationResponse<Employee>(
                    filteredEntities,
                    totalCount,
                    filter.draw,
                    filter.length
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(EmployeeRepository));
                throw;
            }
        }


        public async Task<Employee> Get(object id)
        {
            try
            {
                return await _context.Employee.FindAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(EmployeeRepository));
                throw;
            }
        }

        public async Task<Employee> Update(Employee entity)
        {
            var response = new Employee();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (entity == null)
                        throw new ArgumentNullException(nameof(entity));

                    response = await _context.Employee.FindAsync(entity.Id);
                    if (response != null)
                    {
                        response.LocationName = entity.LocationName;
                        response.DepartmentName = entity.DepartmentName;
                        if (!string.IsNullOrEmpty(entity.CategoryName))
                            response.CategoryName = entity.CategoryName;
                        if (!string.IsNullOrEmpty(entity.SubCategoryName))
                            response.SubCategoryName = entity.SubCategoryName;
                        response.E1 = entity.E1;
                        response.E2 = entity.E2;
                        response.C = entity.C;
                        response.M1 = entity.M1;
                        response.M2 = entity.M2;

                        await _context.SaveChangesAsync();
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(EmployeeRepository));
                    throw;
                }
            }
            return response;
        }

        public async Task Delete(object id)
        {
            try
            {
                var entity = await _context.Employee.FindAsync(id);
                _context.Employee.Remove(entity);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(EmployeeRepository));
                throw;
            }
        }
    }
}
