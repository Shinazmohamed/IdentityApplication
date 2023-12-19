using AutoMapper;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business
{
    public class EmployeeBusiness : IEmployeeBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeBusiness> _logger;

        public EmployeeBusiness(IUnitOfWork unitofwork, IMapper mapper, ILogger<EmployeeBusiness> logger)
        {
            _unitOfWork = unitofwork;
            _mapper = mapper;
            _logger = logger;
        }

        public void Create(InsertEmployeeRequest request)
        {
            try
            {
                var selectedLocation = _unitOfWork.Location.GetLocationById(Guid.Parse(request.SelectedLocation));
                var selectedDepartment = _unitOfWork.Department.GetDepartmentById(Guid.Parse(request.SelectedDepartment));
                var selectedCategory = _unitOfWork.Category.GetCategoryById(Guid.Parse(request.SelectedCategory));
                var selectedSubCategory = _unitOfWork.SubCategory.GetSubCategoryById(Guid.Parse(request.SelectedSubCategory));

                var _entity = _mapper.Map<Employee>(request);
                if (string.IsNullOrEmpty(request.E1) && string.IsNullOrEmpty(request.E2)) _entity.C = null;
                else if (!string.IsNullOrEmpty(request.E1) && !string.IsNullOrEmpty(request.E2)) _entity.C = 2;
                else if (!string.IsNullOrEmpty(request.E1) || !string.IsNullOrEmpty(request.E2)) _entity.C = 1;

                _entity.LocationName = selectedLocation.LocationName;
                _entity.DepartmentName = selectedDepartment.DepartmentName;
                _entity.CategoryName = selectedCategory.CategoryName;
                _entity.SubCategoryName = selectedSubCategory.SubCategoryName;
                _unitOfWork.Employee.Create(_entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(EmployeeBusiness));
            }
        }

        public async Task<PaginationResponse<Employee>> GetAll(PaginationFilter filter)
        {
            var response = new PaginationResponse<Employee>();
            try
            {

                if (!string.IsNullOrEmpty(filter.location))
                    filter.location = _unitOfWork.Location.GetLocationById(Guid.Parse(filter.location))?.LocationName;

                if (!string.IsNullOrEmpty(filter.department))
                    filter.department = _unitOfWork.Department.GetDepartmentById(Guid.Parse(filter.department))?.DepartmentName;

                if (!string.IsNullOrEmpty(filter.category))
                    filter.category = _unitOfWork.Category.GetCategoryById(Guid.Parse(filter.category))?.CategoryName;

                if (!string.IsNullOrEmpty(filter.subcategory))
                    filter.subcategory = _unitOfWork.SubCategory.GetSubCategoryById(Guid.Parse(filter.subcategory))?.SubCategoryName;

                response = await _unitOfWork.Employee.GetEntitiesWithFilters(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(EmployeeBusiness));
            }
            return response;
        }

        public async Task<Employee?> GetById(string id)
        {
            var response = new Employee();
            try
            {
                response = await _unitOfWork.Employee.Get(Guid.Parse(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(EmployeeBusiness));
            }
            return response;
        }

        public async Task Update(InsertEmployeeRequest request, bool isAdmin)
        {
            try
            {
                var _entity = _mapper.Map<Employee>(request);

                if (isAdmin)
                {
                    _entity.LocationName = _unitOfWork.Location.GetLocationById(Guid.Parse(request.SelectedLocation))?.LocationName;
                    _entity.DepartmentName = _unitOfWork.Department.GetDepartmentById(Guid.Parse(request.SelectedDepartment))?.DepartmentName;
                    _entity.CategoryName = _unitOfWork.Category.GetCategoryById(Guid.Parse(request.SelectedCategory))?.CategoryName;
                    _entity.SubCategoryName = _unitOfWork.SubCategory.GetSubCategoryById(Guid.Parse(request.SelectedSubCategory))?.SubCategoryName;
                }
                else
                {
                    var current = await _unitOfWork.Employee.Get(Guid.Parse(request.EmployeeId));
                    _entity.LocationName = current.LocationName;
                    _entity.DepartmentName = current.DepartmentName;
                    _entity.CategoryName = current.CategoryName;
                    _entity.SubCategoryName = current.SubCategoryName;
                }

                if (string.IsNullOrEmpty(request.E1) && string.IsNullOrEmpty(request.E2)) _entity.C = 0;
                else if (!string.IsNullOrEmpty(request.E1) && !string.IsNullOrEmpty(request.E2)) _entity.C = 2;
                else if (!string.IsNullOrEmpty(request.E1) || !string.IsNullOrEmpty(request.E2)) _entity.C = 1;

                _unitOfWork.Employee.Update(_entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(EmployeeBusiness));
            }
        }

        public async Task Delete(string id)
        {
            try
            {
                await _unitOfWork.Employee.Delete(Guid.Parse(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(EmployeeBusiness));
            }
        }

    }
}
