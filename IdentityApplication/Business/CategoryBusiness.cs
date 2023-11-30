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

        public CategoryBusiness(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ListCategoryModel GetCategoryById(string id)
        {
            var category = _unitOfWork.Category.GetCategoryById(Guid.Parse(id));
            return _mapper.Map<ListCategoryModel>(category);
        }

        public async Task<PaginationResponse<ListCategoryModel>> GetAllWithFilters(PaginationFilter filter)
        {
            return await _unitOfWork.Category.GetEntitiesWithFilters(filter);
        }

        public async Task Create(CreateCategoryRequest request)
        {
            var entity = _mapper.Map<Category>(request);
            _unitOfWork.Category.Create(entity);
        }

        public async Task Update(CreateCategoryRequest request)
        {
            var entity = _mapper.Map<Category>(request);
            _unitOfWork.Category.Update(entity);
        }

        public async Task Delete(string id)
        {
            await _unitOfWork.Category.Delete(Guid.Parse(id));
        }
    }
}
