using IdentityApplication.Core.Entities;

namespace IdentityApplication.Core.Contracts
{
    public interface ILocationRepository
    {
        IList<Location> GetLocations();
        Location GetLocationById(Guid Id);
        Location GetLocationByName(string Name);
    }
}
