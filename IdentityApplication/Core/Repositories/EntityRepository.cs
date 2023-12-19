using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using Microsoft.EntityFrameworkCore;

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

        public List<Entity> GetEntities()
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

        public void Edit(Entity entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (entity == null)
                        throw new ArgumentNullException(nameof(entity));

                    var existingMapping = _context.Entity
                        .FirstOrDefault(e => e.EntityId == entity.EntityId);

                    if (existingMapping == null)
                    {
                        throw new ArgumentNullException(nameof(entity));
                    }

                    existingMapping.Name = entity.Name;
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

        public async Task Delete(Guid id)
        {
            try
            {
                var entity = await _context.Entity.FirstOrDefaultAsync(e => e.EntityId == id);
                var permissions = _context.Permission.Where(e => e.EntityId == id).ToList();
                _context.Permission.RemoveRange(permissions);

                _context.Entity.Remove(entity);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(EntityRepository));
                throw;
            }
        }
    }
}
