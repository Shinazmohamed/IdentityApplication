using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace IdentityApplication.Core.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StaffRepository> _logger;

        public StaffRepository(ApplicationDbContext context, ILogger<StaffRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Create(Staff request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var existingMapping = _context.Staffs
                        .FirstOrDefault(e => e.EmployeeCode == request.EmployeeCode && e.LocationId == request.LocationId);
                    if (existingMapping != null)
                    {
                        throw new ArgumentNullException(nameof(existingMapping));
                    }

                    _context.Staffs.Add(request);
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(StaffRepository));
                    throw;
                }
            }
        }

        public async Task<PaginationResponse<ViewStaffResponse>> GetEntitiesWithFilters(PaginationFilter filter)
        {
            try
            {
                var query = _context.Staffs
                    .Include(e => e.Team)
                    .AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filter.location))
                    query = query.Where(e => e.LocationId == Guid.Parse(filter.location));

                // Total count
                var filteredEntities = await query.ToListAsync();
                var totalCount = filteredEntities.Count;

                // Order and pagination
                query = query
                             .Skip(filter.start)
                             .Take(filter.length);

                filteredEntities = await query.ToListAsync();

                var resultViewModel = filteredEntities.Select(entity => new ViewStaffResponse
                {
                    StaffId = entity.StaffId,
                    EmployeeCode = entity.EmployeeCode,
                    Team = entity?.Team?.TeamName
                }).ToList();

                return new PaginationResponse<ViewStaffResponse>(
                    resultViewModel,
                    totalCount,
                    filter.draw,
                    filter.length
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(StaffRepository));
                throw;
            }
        }

        public void Delete(Guid id)
        {
            try
            {

                var entity = _context.Staffs.FirstOrDefault(e => e.StaffId == id);
                _context.Staffs.Remove(entity);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(StaffRepository));
                throw;
            }
        }
    }
}
