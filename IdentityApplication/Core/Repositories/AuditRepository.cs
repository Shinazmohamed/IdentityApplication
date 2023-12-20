using AutoMapper;
using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace IdentityApplication.Core.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuditRepository> _logger;
        private readonly IMapper _mapper;

        public AuditRepository(ApplicationDbContext context, ILogger<AuditRepository> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<ListAuditModel>> GetEntitiesWithFilters(PaginationFilter filter)
        {
            var response = new PaginationResponse<ListAuditModel>();
            try
            {
                var query = _context.AuditLogs.OrderByDescending(a => a.DateTime).AsQueryable();
                var totalCount = await query.CountAsync();

                var filteredEntities = await query
                    .Skip(filter.start)
                    .Take(filter.length)
                    .ToListAsync();

                // Take the last 250 transactions after sorting
                var resultViewModel = filteredEntities
                    .TakeLast(250)
                    .Select(entity => _mapper.Map<ListAuditModel>(entity))
                    .ToList();

                response.Data = resultViewModel;
                response.TotalCount = totalCount;
                response.CurrentPage = filter.draw;
                response.PageSize = filter.length;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(AuditRepository));
            }
            return response;
        }
    }
}
