using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
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
        public async Task<PaginationResponse<Entity>> GetEntitiesWithFilters(PaginationFilter filter)
        {
            var response = new PaginationResponse<Entity>();
            try
            {

                var query = _context.Entity.OrderBy(e => e.EntityId).AsQueryable();

                //if (!string.IsNullOrEmpty(filter.entity))
                //{
                //    query = query.Where(e => e.Entity.Name == filter.entity);
                //}

                var totalCount = await query.CountAsync();
                var filteredEntities = await query.Skip(filter.start).Take(filter.length).Include(e => e.Permissions).ToListAsync();

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

        public async Task<PaginationResponse<Permission>> GetPermissionsWithFilters(PaginationFilter filter)
        {
            var response = new PaginationResponse<Permission>();
            try
            {

                var query = _context.Permission.OrderBy(e => e.Id).AsQueryable();

                var totalCount = await query.CountAsync();
                var filteredEntities = await query.Skip(filter.start).Take(filter.length).Include(e => e.Entity).ToListAsync();

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

        public void Create(Entities.Permission request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var existingMapping = _context.Permission
                        .FirstOrDefault(e => e.Value == request.Value);
                    if (existingMapping != null)
                    {
                        throw new ArgumentNullException(nameof(existingMapping));
                    }

                    _context.Permission.Add(request);
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(PermissionRepository));
                    throw;
                }
            }
        }
        public async Task Delete(Guid id)
        {
            try
            {
                var entity = await _context.Permission.FirstOrDefaultAsync(e => e.Id == id);
                _context.Permission.Remove(entity);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(PermissionRepository));
                throw;
            }
        }
        public void Update(Entities.Permission entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (entity == null)
                        throw new ArgumentNullException(nameof(entity));

                    var existingMapping = _context.Permission
                        .FirstOrDefault(e => e.Id == entity.Id);

                    if (existingMapping == null)
                    {
                        throw new ArgumentNullException(nameof(entity));
                    }

                    existingMapping.Value = entity.Value;
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(PermissionRepository));
                    throw;
                }
            }
        }
    }
}
