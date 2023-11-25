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

        public SubCategoryBusiness(IUnitOfWork unitofwork, IMapper mapper)
        {
            _unitOfWork = unitofwork;
            _mapper = mapper;
        }

        public async Task CreateMapping(CreateSubCategoryRequest request)
        {
            var entity = _mapper.Map<CategoryMapping>(request);
            _unitOfWork.SubCategory.CreateMapping(entity);
        }

        public async Task UpdateMapping(CreateSubCategoryRequest request)
        {
            var entity = _mapper.Map<CategoryMapping>(request);
            _unitOfWork.SubCategory.UpdateMapping(entity);
        }

        public async Task DeleteMapping(string id)
        {
            await _unitOfWork.SubCategory.DeleteMapping(Guid.Parse(id));
        }

        public async Task<PaginationResponse<ListCategoryMappingModel>> GetAll(PaginationFilter filter)
        {
            return await _unitOfWork.SubCategory.GetEntitiesWithFilters(filter);
        }
    }
}
