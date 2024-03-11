using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityApplication.Core.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TeamRepository> _logger;

        public TeamRepository(ApplicationDbContext context, ILogger<TeamRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Team> GetAll()
        {
            try
            {
                var query = _context.Teams.AsNoTracking().AsQueryable();
                return query.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(TeamRepository));
                throw;
            }
        }
    }
}
