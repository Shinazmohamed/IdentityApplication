using AutoMapper;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
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
    }
}
