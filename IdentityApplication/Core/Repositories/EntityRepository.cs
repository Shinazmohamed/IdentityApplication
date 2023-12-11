using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;

namespace IdentityApplication.Core.Repositories
{
    public class EntityRepository : IEntityRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EntityRepository> _logger;
        public EntityRepository(ApplicationDbContext context, ILogger<EntityRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<Entity> GetEntities()
        {
            var response = new List<Entity>();
            try
            {
                response = _context.Entity.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(EntityRepository));
            }
            return response;
        }


        public void Create(Entity request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var existingMapping = _context.Entity
                        .FirstOrDefault(e => e.Name == request.Name);
                    if (existingMapping != null)
                    {
                        throw new ArgumentNullException(nameof(existingMapping));
                    }

                    _context.Entity.Add(request);
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(EntityRepository));
                    throw;
                }
            }
        }
    }
}
