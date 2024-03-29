﻿using AutoMapper;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business
{
    public class CategoryDepartmentMappingBusiness : ICategoryDepartmentMappingBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryDepartmentMappingBusiness> _logger;

        public CategoryDepartmentMappingBusiness(IUnitOfWork unitofwork, IMapper mapper, ILogger<CategoryDepartmentMappingBusiness> logger)
        {
            _unitOfWork = unitofwork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateMapping(CreateCategoryDepartmentMappingViewModel request)
        {
            try
            {
                var entity = _mapper.Map<DepartmentCategory>(request);
                _unitOfWork.CategoryDepartmentMapping.Create(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(CategoryBusiness));
            }
        }

        public async Task UpdateMapping(CreateCategoryDepartmentMappingViewModel request)
        {
            try
            {
                var entity = new DepartmentCategory()
                {
                    CategoryId = Guid.Parse(request.SelectedCategoryId),
                    DepartmentId = Guid.Parse(request.SelectedDepartment)
                };
                _unitOfWork.CategoryDepartmentMapping.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(CategoryBusiness));
            }
        }

        public void DeleteMapping(string categoryId)
        {
            try
            {
                _unitOfWork.CategoryDepartmentMapping.Delete(Guid.Parse(categoryId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(CategoryBusiness));
            }
        }

        public async Task<PaginationResponse<ListCategoryDepartmentMappingViewModel>> GetAllWithFilters(PaginationFilter filter)
        {
            var response = new PaginationResponse<ListCategoryDepartmentMappingViewModel>();
            try
            {
                response = await _unitOfWork.CategoryDepartmentMapping.GetEntitiesWithFilters(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(CategoryBusiness));
            }
            return response;
        }
    }
}
