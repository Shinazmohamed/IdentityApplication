using AutoMapper;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business
{
    public class DepartmentBusiness : IDepartmentBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentBusiness(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ListDepartmentViewModel GetDepartmentById(string id)
        {
            var department = _unitOfWork.Department.GetDepartmentById(Guid.Parse(id));
            return _mapper.Map<ListDepartmentViewModel>(department);
        }

        public async Task<PaginationResponse<ListDepartmentViewModel>> GetAllWithFilters(PaginationFilter filter)
        {
            return await _unitOfWork.Department.GetEntitiesWithFilters(filter);
        }

        public async Task Create(CreateDepartmentViewModel request)
        {
            var entity = _mapper.Map<Department>(request);
            _unitOfWork.Department.Create(entity);
        }

        public async Task Update(CreateDepartmentViewModel request)
        {
            var entity = _mapper.Map<Department>(request);
            _unitOfWork.Department.Update(entity);
        }

        public async Task Delete(string id)
        {
            await _unitOfWork.Department.Delete(Guid.Parse(id));
        }
    }
}
