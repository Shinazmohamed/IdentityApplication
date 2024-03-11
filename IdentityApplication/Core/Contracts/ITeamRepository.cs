using IdentityApplication.Core.Entities;

namespace IdentityApplication.Core.Contracts
{
    public interface ITeamRepository
    {
        List<Team> GetAll();
    }
}
