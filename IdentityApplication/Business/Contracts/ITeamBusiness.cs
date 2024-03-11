using IdentityApplication.Core.Entities;

namespace IdentityApplication.Business.Contracts
{
    public interface ITeamBusiness
    {
        List<Team> GetAll();
    }
}
