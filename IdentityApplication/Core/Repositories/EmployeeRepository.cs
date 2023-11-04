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

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        protected string GetFullErrorTextAndRollbackEntityChanges(DbUpdateException exception)
        {
            if (_context is DbContext dbContext)
            {
                var entries = dbContext.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

                entries.ForEach(entry =>
                {
                    try
                    {
                        entry.State = EntityState.Unchanged;
                    }
                    catch (Exception)
                    {
                        //ignored
                    }
                });
            }

            try
            {
                _context.SaveChanges();
                return exception.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public void Create(Employee employee)
        {
            _context.Employee.Add(employee);
            _context.SaveChanges();
        }

        public async Task<PaginationResponse<Employee>> GetEntitiesWithFilters(PaginationFilter filter)
        {
            var query = _context.Employee.AsQueryable(); // Use the DbSet from your context

            // Perform the count and pagination
            var totalCount = await query.CountAsync(); // Use CountAsync for better performance
            var filteredEntities = await query.Skip(filter.start).Take(filter.length).ToListAsync();

            return new PaginationResponse<Employee>(
                filteredEntities,
                totalCount,
                filter.draw,
                filter.length
            );
        }

        public async Task<Employee> Get(object id)
        {
            try
            {
                return await _context.Employee.FindAsync(id);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(ex), ex.InnerException);
            }
        }

        public void Update(Employee entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                _context.Employee.Update(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(ex), ex.InnerException);
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
            catch (DbUpdateException ex)
            {
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(ex), ex.InnerException);
            }
        }
    }
}
