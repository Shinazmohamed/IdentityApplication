using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business
{
    public class EmployeeBusiness : IEmployeeBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeBusiness(IUnitOfWork unitofwork)
        {
            _unitOfWork = unitofwork;
        }

        public void Create(Employee request)
        {
            if (string.IsNullOrEmpty(request.E1) && string.IsNullOrEmpty(request.E2)) request.C = 0;
            else if (!string.IsNullOrEmpty(request.E1) && !string.IsNullOrEmpty(request.E2)) request.C = 2;
            else if (!string.IsNullOrEmpty(request.E1) || !string.IsNullOrEmpty(request.E2)) request.C = 1;
            _unitOfWork.Employee.Create(request);
        }

        public async Task<PaginationResponse<Employee>> GetAll(PaginationFilter filter)
        {
            return await _unitOfWork.Employee.GetEntitiesWithFilters(filter);
        }

        public async Task<Employee?> GetById(string id)
        {
            return await _unitOfWork.Employee.Get(Guid.Parse(id));
        }

        public void Update(Employee request)
        {
            _unitOfWork.Employee.Update(request);
        }

        public async Task Delete(string id)
        {
            await _unitOfWork.Employee.Delete(Guid.Parse(id));
        }

    }
}
