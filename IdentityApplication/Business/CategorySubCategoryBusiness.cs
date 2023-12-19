using AutoMapper;
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
        private readonly ILogger<CategorySubCategoryBusiness> _logger;
        public CategorySubCategoryBusiness(IUnitOfWork unitofwork, IMapper mapper, ILogger<CategorySubCategoryBusiness> logger)
        {
            _unitOfWork = unitofwork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateMapping(CreateCategorySubCategoryRequest request)
        {
            try
            {
                var entity = _mapper.Map<SubCategory>(request);
                _unitOfWork.CategorySubCategory.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(CategorySubCategoryBusiness));
            }
        }

        public async Task UpdateMapping(CreateCategorySubCategoryRequest request)
        {
            try
            {
                var entity = new SubCategory()
                {
                    SubCategoryId = Guid.Parse(request.SelectedSubCategoryId),
                    CategoryId = Guid.Parse(request.SelectedCategory)
                };
                _unitOfWork.CategorySubCategory.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(CategorySubCategoryBusiness));
            }
        }

        public async Task DeleteMapping(string subCategoryId)
        {
            try
            {
                var entity = new SubCategory()
                {
                    SubCategoryId = Guid.Parse(subCategoryId),
                    CategoryId = null
                };
                _unitOfWork.CategorySubCategory.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(CategorySubCategoryBusiness));
            }
        }

        public async Task<PaginationResponse<ListCategorySubCategoryModel>> GetAll(PaginationFilter filter)
        {
            var response = new PaginationResponse<ListCategorySubCategoryModel>();
            try
            {
                response = await _unitOfWork.CategorySubCategory.GetEntitiesWithFilters(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(CategorySubCategoryBusiness));
            }
            return response;
        }
    }
}
