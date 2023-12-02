﻿using AutoMapper;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business
{
    public class CategorySubCategoryBusiness : ICategorySubCategoryBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategorySubCategoryBusiness(IUnitOfWork unitofwork, IMapper mapper)
        {
            _unitOfWork = unitofwork;
            _mapper = mapper;
        }

        public async Task UpdateMapping(CreateCategorySubCategoryRequest request)
        {
            var entity = _mapper.Map<SubCategory>(request);
            _unitOfWork.CategorySubCategory.Update(entity);
        }

        public async Task<PaginationResponse<ListCategorySubCategoryModel>> GetAll(PaginationFilter filter)
        {
            return await _unitOfWork.CategorySubCategory.GetEntitiesWithFilters(filter);
        }
    }
}