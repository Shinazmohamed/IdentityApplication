using AutoMapper;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business
{
    public class CategoryBusiness : ICategoryBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryBusiness> _logger;

        public CategoryBusiness(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CategoryBusiness> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public ListCategoryModel GetCategoryById(string id)
        {
            var response = new ListCategoryModel();
            try
            {
                var category = _unitOfWork.Category.GetCategoryById(Guid.Parse(id));
                response = _mapper.Map<ListCategoryModel>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(CategoryBusiness));
            }
            return response;
        }

        public async Task<PaginationResponse<ListCategoryModel>> GetAllWithFilters(PaginationFilter filter)
        {
            var response = new PaginationResponse<ListCategoryModel>();
            try
            {
                response = await _unitOfWork.Category.GetEntitiesWithFilters(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(CategoryBusiness));
            }
            return response;
        }

        public async Task Create(CreateCategoryRequest request)
        {
            try
            {
                var entity = _mapper.Map<Category>(request);
                _unitOfWork.Category.Create(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(CategoryBusiness));
            }
        }

        public async Task Update(CreateCategoryRequest request)
        {
            try
            {
                var entity = _mapper.Map<Category>(request);
                _unitOfWork.Category.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(CategoryBusiness));
            }
        }

        public async Task Delete(string id)
        {
            try
            {
                await _unitOfWork.Category.Delete(Guid.Parse(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(CategoryBusiness));
            }
        }

        public List<ListCategoryModel> GetCategoryByDepartmentId(string id)
        {
            var response = new List<ListCategoryModel>();
            try
            {
                var category = _unitOfWork.Category.GetCategoryByDepartmentId(Guid.Parse(id));
                response = _mapper.Map<List<ListCategoryModel>>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(CategoryBusiness));
            }
            return response;
        }
    }
}
