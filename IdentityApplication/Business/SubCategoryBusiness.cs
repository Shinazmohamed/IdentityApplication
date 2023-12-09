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

        public SubCategoryBusiness(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Create(CreateSubCategoryRequest request)
        {
            var entity = _mapper.Map<SubCategory>(request);
            _unitOfWork.SubCategory.Create(entity);
        }

        public async Task Update(CreateSubCategoryRequest request)
        {
            var entity = _mapper.Map<SubCategory>(request);
            _unitOfWork.SubCategory.Update(entity);
        }

        public async Task Delete(string id)
        {
            await _unitOfWork.SubCategory.Delete(Guid.Parse(id));
        }

        public async Task<PaginationResponse<ListSubCategoryModel>> GetAllWithFilters(PaginationFilter filter)
        {
            return await _unitOfWork.SubCategory.GetEntitiesWithFilters(filter);
        }

        public List<ListSubCategoryModel> GetSubCategoriesById(string Id)
        {
            var subcategories = _unitOfWork.SubCategory.GetSubCategoryById(Guid.Parse(Id));
            return _mapper.Map<List<ListSubCategoryModel>>(subcategories);
        }

        public List<ListSubCategoryModel> GetSubCategoriesByCategoryId(string Id)
        {
            var subcategories = _unitOfWork.SubCategory.GetSubCategoryByCategoryId(Guid.Parse(Id));
            return _mapper.Map<List<ListSubCategoryModel>>(subcategories);
        }
    }
}
