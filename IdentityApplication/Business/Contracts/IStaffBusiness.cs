using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface IStaffBusiness
    {
        Task CreateAsync(CreateStaffRequest request);
        Task<PaginationResponse<ViewStaffResponse>> GetAll(PaginationFilter filter);
        void Delete(string staffId);
    }
}
