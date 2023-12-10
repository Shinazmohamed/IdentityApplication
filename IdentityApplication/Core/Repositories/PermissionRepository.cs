using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace IdentityApplication.Core.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PermissionRepository> _logger;
        public PermissionRepository(ApplicationDbContext context, ILogger<PermissionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<PaginationResponse<Entities.Permission>> GetEntitiesWithFilters(PaginationFilter filter)
        {
            var response = new PaginationResponse<Entities.Permission>();
            try
            {
                var query = _context.Permission.OrderBy(e => e.Id).AsQueryable();

                if (!string.IsNullOrEmpty(filter.entity))
                {
                    query = query.Where(e => e.Entity == filter.entity);
                }

                var totalCount = await query.CountAsync();
                var filteredEntities = await query.Skip(filter.start).Take(filter.length).ToListAsync();
                
                response.Data = filteredEntities;
                response.CurrentPage = filter.draw;
                response.PageSize = filter.length;
                response.TotalCount = totalCount;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(PermissionRepository));
            }
            return response;
        }
    }
}
