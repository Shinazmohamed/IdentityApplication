using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;

namespace IdentityApplication.Core.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext _context;

        public LocationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<Location> GetLocations()
        {
            return _context.Location.ToList();
        }
    }
}
