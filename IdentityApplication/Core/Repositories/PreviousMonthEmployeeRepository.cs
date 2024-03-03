using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace IdentityApplication.Core.Repositories
{
    public class PreviousMonthEmployeeRepository : IPreviousMonthEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PreviousMonthEmployeeRepository> _logger;
        public PreviousMonthEmployeeRepository(ApplicationDbContext context, ILogger<PreviousMonthEmployeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<PaginationResponse<PreviousMonthEmployee>> GetEntitiesWithFilters(PaginationFilter filter)
        {
            try
            {
                var query = _context.PreviousMonthEmployees.AsNoTracking().AsQueryable();

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
                var filteredEntities = await query.ToListAsync();
                var totalCount = filteredEntities.Count;

                // Order and pagination
                query = query.OrderBy(e => e.LocationName)
                             .ThenBy(e => e.DepartmentName)
                             .ThenBy(e => e.CategoryName)
                             .ThenBy(e => e.SubCategoryName)
                             .Skip(filter.start)
                             .Take(filter.length);

                filteredEntities = await query.ToListAsync();

                return new PaginationResponse<PreviousMonthEmployee>(
                    filteredEntities,
                    totalCount,
                    filter.draw,
                    filter.length
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(PreviousMonthEmployeeRepository));
                throw;
            }
        }


        public async Task<PreviousMonthEmployee> Get(object id)
        {
            try
            {
                return await _context.PreviousMonthEmployees.FindAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(PreviousMonthEmployeeRepository));
                throw;
            }
        }

        public async Task<PreviousMonthEmployee> Update(Employee entity)
        {
            var response = new PreviousMonthEmployee();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (entity == null)
                        throw new ArgumentNullException(nameof(entity));

                    response = await _context.PreviousMonthEmployees.FindAsync(entity.Id);
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
                    _logger.LogError(e, "{Repo} All function error", typeof(PreviousMonthEmployeeRepository));
                    throw;
                }
            }
            return response;
        }

        public async Task Delete(object id)
        {
            try
            {
                var entity = await _context.PreviousMonthEmployees.FindAsync(id);
                _context.PreviousMonthEmployees.Remove(entity);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(PreviousMonthEmployeeRepository));
                throw;
            }
        }

        public void Create(PreviousMonthEmployee employee)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.PreviousMonthEmployees.Add(employee);
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(PreviousMonthEmployeeRepository));
                    throw;
                }
            }
        }
    }
}
