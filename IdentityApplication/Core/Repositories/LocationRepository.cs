using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityApplication.Core.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LocationRepository> _logger;

        public LocationRepository(ApplicationDbContext context, IMemoryCache cache, IConfiguration configuration, ILogger<LocationRepository> logger)
        {
            _context = context;
            _cache = cache;
            _configuration = configuration;
            _logger = logger;
        }
        public IList<Location> GetLocations()
        {
            var response = new List<Location>();
            try
            {
                var cacheSettings = _configuration.GetSection("AppSettings:CacheSettings");
                bool enableCache = cacheSettings.GetValue<bool>("EnableCache");
                int cacheDurationMinutes = cacheSettings.GetValue<int>("CacheDurationMinutes");

                const string cacheKey = "Locations";

                if (_cache.TryGetValue(cacheKey, out IList<Location> cachedLocations))
                {
                    return cachedLocations;
                }

                response = _context.Location.ToList();

                _cache.Set(cacheKey, response, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheDurationMinutes)
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(LocationRepository));
            }
            return response;
        }
        public Location GetLocationById(Guid Id)
        {
            var response = new Location();

            try
            {
                response = _context.Location.FirstOrDefault(l => l.LocationId == Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(LocationRepository));
            }
            return response;
        }
        public Location GetLocationByName(string Name)
        {
            var response = new Location();
            try
            {
                response = _context.Location.FirstOrDefault(l => l.LocationName == Name);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(LocationRepository));
            }
            return response;
        }
    }
}
