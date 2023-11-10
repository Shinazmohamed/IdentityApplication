﻿using AutoMapper;
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

        public EmployeeBusiness(IUnitOfWork unitofwork, IMapper mapper)
        {
            _unitOfWork = unitofwork;
            _mapper = mapper;
        }

        public void Create(InsertEmployeeRequest request)
        {
            var selectedLocation = _unitOfWork.Location.GetLocationById(Guid.Parse(request.SelectedLocation));
            var selectedDepartment = _unitOfWork.Department.GetDepartmentById(Guid.Parse(request.SelectedDepartment));
            var selectedCategory = _unitOfWork.Category.GetCategoryById(Guid.Parse(request.SelectedCategory));
            var selectedSubCategory = _unitOfWork.SubCategory.GetSubCategoryById(Guid.Parse(request.SelectedSubCategory));

            var _entity = _mapper.Map<Employee>(request);
            if (string.IsNullOrEmpty(request.E1) && string.IsNullOrEmpty(request.E2)) _entity.C = 0;
            else if (!string.IsNullOrEmpty(request.E1) && !string.IsNullOrEmpty(request.E2)) _entity.C = 2;
            else if (!string.IsNullOrEmpty(request.E1) || !string.IsNullOrEmpty(request.E2)) _entity.C = 1;

            _entity.LocationName = selectedLocation.LocationName;
            _entity.DepartmentName = selectedDepartment.DepartmentName;
            _entity.CategoryName = selectedCategory.CategoryName;
            _entity.SubCategoryName = selectedSubCategory.SubCategoryName;
            _unitOfWork.Employee.Create(_entity);
        }

        public async Task<PaginationResponse<Employee>> GetAll(PaginationFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.location))
                filter.location = _unitOfWork.Location.GetLocationById(Guid.Parse(filter.location))?.LocationName;

            if (!string.IsNullOrEmpty(filter.department))
                filter.department = _unitOfWork.Department.GetDepartmentById(Guid.Parse(filter.department))?.DepartmentName;

            if (!string.IsNullOrEmpty(filter.category))
                filter.category = _unitOfWork.Category.GetCategoryById(Guid.Parse(filter.category))?.CategoryName;

            if (!string.IsNullOrEmpty(filter.subcategory))
                filter.subcategory = _unitOfWork.SubCategory.GetSubCategoryById(Guid.Parse(filter.subcategory))?.SubCategoryName;

            return await _unitOfWork.Employee.GetEntitiesWithFilters(filter);
        }

        public async Task<Employee?> GetById(string id)
        {
            return await _unitOfWork.Employee.Get(Guid.Parse(id));
        }

        public void Update(InsertEmployeeRequest request)
        {
            var selectedLocation = _unitOfWork.Location.GetLocationById(Guid.Parse(request.SelectedLocation));
            var selectedDepartment = _unitOfWork.Department.GetDepartmentById(Guid.Parse(request.SelectedDepartment));
            var selectedCategory = _unitOfWork.Category.GetCategoryById(Guid.Parse(request.SelectedCategory));
            var selectedSubCategory = _unitOfWork.SubCategory.GetSubCategoryById(Guid.Parse(request.SelectedSubCategory));

            var _entity = _mapper.Map<Employee>(request);
            if (string.IsNullOrEmpty(request.E1) && string.IsNullOrEmpty(request.E2)) _entity.C = 0;
            else if (!string.IsNullOrEmpty(request.E1) && !string.IsNullOrEmpty(request.E2)) _entity.C = 2;
            else if (!string.IsNullOrEmpty(request.E1) || !string.IsNullOrEmpty(request.E2)) _entity.C = 1;

            _entity.LocationName = selectedLocation.LocationName;
            _entity.DepartmentName = selectedDepartment.DepartmentName;
            _entity.CategoryName = selectedCategory.CategoryName;
            _entity.SubCategoryName = selectedSubCategory.SubCategoryName;

            _unitOfWork.Employee.Update(_entity);
        }

        public async Task Delete(string id)
        {
            await _unitOfWork.Employee.Delete(Guid.Parse(id));
        }

    }
}