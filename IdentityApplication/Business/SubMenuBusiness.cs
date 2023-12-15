using AutoMapper;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business
{
    public class SubMenuBusiness : ISubMenuBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SubMenuBusiness(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<SubMenu> GetAll()
        {
            return _unitOfWork.SubMenu.GetAll();
        }
        public void Create(CreateMenuRequest request)
        {
            var entity = _mapper.Map<SubMenu>(request);
            _unitOfWork.SubMenu.Create(entity);
        }
        public void Update(ManageMenuViewModel request)
        {
            _unitOfWork.SubMenu.Update(request);
        }
        public PaginationResponse<SubMenuViewModel> GetSubMenusWithFilters(PaginationFilter filter)
        {
            return _unitOfWork.SubMenu.GetSubMenusWithFilters(filter);
        }
        public void Edit(CreateMenuRequest request)
        {
            _unitOfWork.SubMenu.Edit(request);
        }
        public async Task Delete(string id)
        {
            await _unitOfWork.SubMenu.Delete(Guid.Parse(id));
        }
    }
}
