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
        private readonly ILogger<DepartmentBusiness> _logger;

        public DepartmentBusiness(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DepartmentBusiness> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public ListDepartmentViewModel GetDepartmentById(string id)
        {
            var response = new ListDepartmentViewModel();
            try
            {
                var department = _unitOfWork.Department.GetDepartmentById(Guid.Parse(id));
                response = _mapper.Map<ListDepartmentViewModel>(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(DepartmentBusiness));
            }
            return response;
        }

        public async Task<PaginationResponse<ListDepartmentViewModel>> GetAllWithFilters(PaginationFilter filter)
        {
            var response = new PaginationResponse<ListDepartmentViewModel>();
            try
            {
                response = await _unitOfWork.Department.GetEntitiesWithFilters(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(DepartmentBusiness));
            }
            return response;
        }

        public async Task Create(CreateDepartmentViewModel request)
        {
            try
            {
                var entity = _mapper.Map<Department>(request);
                _unitOfWork.Department.Create(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(DepartmentBusiness));
            }
        }

        public async Task Update(CreateDepartmentViewModel request)
        {
            try
            {
                var entity = _mapper.Map<Department>(request);
                _unitOfWork.Department.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(DepartmentBusiness));
            }
        }

        public async Task Delete(string id)
        {
            try
            {
                await _unitOfWork.Department.Delete(Guid.Parse(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(DepartmentBusiness));
            }
        }
    }
}
