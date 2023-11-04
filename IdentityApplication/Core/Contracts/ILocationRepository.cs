using IdentityApplication.Core.Entities;

namespace IdentityApplication.Core.Contracts
{
    public interface ILocationRepository
    {
        IList<Location> GetLocations();
    }
}
