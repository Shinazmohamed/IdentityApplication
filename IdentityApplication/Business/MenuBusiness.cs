using AutoMapper;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business
{
    public class MenuBusiness : IMenuBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MenuBusiness(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<MenuViewModel> GetMenus(Guid? roleId)
        {
            var menus = _unitOfWork.Menu.GetMenus();
            var mapped = _mapper.Map<List<MenuViewModel>>(menus);

            return mapped;
        }
    }
}
