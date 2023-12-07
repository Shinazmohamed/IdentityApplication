using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business
{
    public class AuditBusiness : IAuditBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuditBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationResponse<ListAuditModel>> GetAllWithFilters(PaginationFilter filter)
        {
            return await _unitOfWork.Audit.GetEntitiesWithFilters(filter);
        }
    }
}
