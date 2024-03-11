using AutoMapper;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;

namespace IdentityApplication.Business
{
    public class TeamBusiness : ITeamBusiness
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TeamBusiness> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public TeamBusiness(IMapper mapper, ILogger<TeamBusiness> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public List<Team> GetAll()
        {
            var response = new List<Team>();
            try
            {
                response = _unitOfWork.Team.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(TeamBusiness));
                throw ex;
            }
            return response;
        }
    }
}
