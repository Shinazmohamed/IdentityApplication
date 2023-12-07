using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface IDepartmentBusiness
    {
        ListDepartmentViewModel GetDepartmentById(string id);
        Task<PaginationResponse<ListDepartmentViewModel>> GetAllWithFilters(PaginationFilter filter);
        Task Create(CreateDepartmentViewModel request);
        Task Update(CreateDepartmentViewModel request);
        Task Delete(string id);
    }
}
