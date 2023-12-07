using AutoMapper;
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

        public CategoryDepartmentMappingBusiness(IUnitOfWork unitofwork, IMapper mapper)
        {
            _unitOfWork = unitofwork;
            _mapper = mapper;
        }

        public async Task CreateMapping(CreateCategoryDepartmentMappingViewModel request)
        {
            var entity = _mapper.Map<Category>(request);
            _unitOfWork.CategoryDepartmentMapping.Update(entity);
        }

        public async Task UpdateMapping(CreateCategoryDepartmentMappingViewModel request)
        {
            var entity = new Category()
            {
                CategoryId = Guid.Parse(request.SelectedCategoryId),
                DepartmentId = (string.IsNullOrWhiteSpace(request.SelectedDepartment)) ? null : Guid.Parse(request.SelectedDepartment)
            };
            _unitOfWork.CategoryDepartmentMapping.Update(entity);
        }

        public void DeleteMapping(string categoryId)
        {
            var entity = new Category()
            {
                CategoryId = Guid.Parse(categoryId),
                DepartmentId = null
            };
            _unitOfWork.CategoryDepartmentMapping.Update(entity);
        }

        public async Task<PaginationResponse<ListCategoryDepartmentMappingViewModel>> GetAllWithFilters(PaginationFilter filter)
        {
            return await _unitOfWork.CategoryDepartmentMapping.GetEntitiesWithFilters(filter);
        }
    }
}
