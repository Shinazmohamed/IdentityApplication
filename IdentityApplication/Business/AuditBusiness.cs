using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Repositories;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business
{
    public class AuditBusiness : IAuditBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuditBusiness> _logger;

        public AuditBusiness(IUnitOfWork unitOfWork, ILogger<AuditBusiness> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<PaginationResponse<ListAuditModel>> GetAllWithFilters(PaginationFilter filter)
        {
            var response = new PaginationResponse<ListAuditModel>();
            try
            {
                response = await _unitOfWork.Audit.GetEntitiesWithFilters(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(AuditBusiness));
            }
            return response;
        }
    }
}
