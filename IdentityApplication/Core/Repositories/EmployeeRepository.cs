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
                var query = _context.Employee.OrderBy(e => e.Id).AsQueryable(); // Use the DbSet from your context

                if (!string.IsNullOrEmpty(filter.location))
                {
                    query = query.Where(e => e.LocationName == filter.location);
                }

                if (!string.IsNullOrEmpty(filter.department))
                {
                    query = query.Where(e => e.DepartmentName == filter.department);
                }

                if (!string.IsNullOrEmpty(filter.category))
                {
                    query = query.Where(e => e.CategoryName == filter.category);
                }

                if (!string.IsNullOrEmpty(filter.subcategory))
                {
                    query = query.Where(e => e.SubCategoryName == filter.subcategory);
                }

                // Perform the count and pagination
                var totalCount = await query.CountAsync();
                var filteredEntities = await query.Skip(filter.start).Take(filter.length).ToListAsync();

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

        public void Update(Employee entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (entity == null)
                        throw new ArgumentNullException(nameof(entity));

                    var existingEmployee = _context.Employee.Find(entity.Id); // Assuming Id is the primary key
                    if (existingEmployee != null)
                    {
                        existingEmployee.LocationName = entity.LocationName;
                        existingEmployee.DepartmentName = entity.DepartmentName;
                        existingEmployee.CategoryName = entity.CategoryName;
                        existingEmployee.SubCategoryName = entity.SubCategoryName;
                        existingEmployee.E1 = entity.E1;
                        existingEmployee.E2 = entity.E2;
                        existingEmployee.C = entity.C;
                        existingEmployee.M1 = entity.M1;
                        existingEmployee.M2 = entity.M2;

                        _context.SaveChanges();

                        transaction.Commit();
                    }

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(EmployeeRepository));
                    throw;
                }
            }
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
