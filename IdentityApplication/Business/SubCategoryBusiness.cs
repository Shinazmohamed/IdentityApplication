using AutoMapper;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business
{
    public class SubCategoryBusiness : ISubCategoryBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SubCategoryBusiness> _logger;


        public SubCategoryBusiness(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SubCategoryBusiness> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public void Create(CreateSubCategoryRequest request)
        {
            try
            {
                var entity = _mapper.Map<SubCategory>(request);
                _unitOfWork.SubCategory.Create(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(SubCategoryBusiness));
            }
        }

        public async Task Update(CreateSubCategoryRequest request)
        {
            try
            {
                var entity = _mapper.Map<SubCategory>(request);
                _unitOfWork.SubCategory.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(SubCategoryBusiness));
            }
        }

        public async Task Delete(string id)
        {
            try
            {
                await _unitOfWork.SubCategory.Delete(Guid.Parse(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(SubCategoryBusiness));
            }
        }

        public async Task<PaginationResponse<ListSubCategoryModel>> GetAllWithFilters(PaginationFilter filter)
        {
            var response = new PaginationResponse<ListSubCategoryModel>();
            try
            {
                response = await _unitOfWork.SubCategory.GetEntitiesWithFilters(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(SubCategoryBusiness));
            }
            return response;
        }

        public List<ListSubCategoryModel> GetSubCategoriesById(string Id)
        {
            var response = new List<ListSubCategoryModel>();
            try
            {
                var subcategories = _unitOfWork.SubCategory.GetSubCategoryById(Guid.Parse(Id));
                response = _mapper.Map<List<ListSubCategoryModel>>(subcategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(SubCategoryBusiness));
            }
            return response;
        }

        public List<ListSubCategoryModel> GetSubCategoriesByCategoryId(string Id)
        {
            var response = new List<ListSubCategoryModel>();

            try
            {
                var subcategories = _unitOfWork.SubCategory.GetSubCategoryByCategoryId(Guid.Parse(Id));
                response = _mapper.Map<List<ListSubCategoryModel>>(subcategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(SubCategoryBusiness));
            }
            return response;
        }
    }
}
