using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using System.Linq;

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
        public Location GetLocationById(Guid Id)
        {
            return _context.Location.FirstOrDefault(l => l.Id == Id);
        }
        public Location GetLocationByName(string Name)
        {
            return _context.Location.FirstOrDefault(l => l.Name == Name);
        }
    }
}
