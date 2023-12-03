using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace IdentityApplication.Core.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public LocationRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public IList<Location> GetLocations()
        {
            const string cacheKey = "Locations";

            if (_cache.TryGetValue(cacheKey, out IList<Location> cachedLocations))
            {
                return cachedLocations;
            }

            var locations = _context.Location.ToList();

            _cache.Set(cacheKey, locations, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

            return locations;
        }
        public Location GetLocationById(Guid Id)
        {
            return _context.Location.FirstOrDefault(l => l.LocationId == Id);
        }
        public Location GetLocationByName(string Name)
        {
            return _context.Location.FirstOrDefault(l => l.LocationName == Name);
        }
    }
}
