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
        private readonly ILogger<SubMenuBusiness> _logger;

        public SubMenuBusiness(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SubMenuBusiness> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public List<SubMenu> GetAll()
        {
            var response = new List<SubMenu>();
            try
            {
                response = _unitOfWork.SubMenu.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(SubMenuBusiness));
            }
            return response;
        }
        public void Create(CreateMenuRequest request)
        {
            try
            {
                var entity = _mapper.Map<SubMenu>(request);
                _unitOfWork.SubMenu.Create(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(SubMenuBusiness));
            }
        }
        public void Update(ManageMenuViewModel request)
        {
            try
            {
                _unitOfWork.SubMenu.Update(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(SubMenuBusiness));
            }
        }
        public PaginationResponse<SubMenuViewModel> GetSubMenusWithFilters(PaginationFilter filter)
        {
            var response = new PaginationResponse<SubMenuViewModel>();
            try
            {
                response = _unitOfWork.SubMenu.GetSubMenusWithFilters(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(SubMenuBusiness));
            }
            return response;
        }
        public void Edit(CreateMenuRequest request)
        {
            try
            {
                _unitOfWork.SubMenu.Edit(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(SubMenuBusiness));
            }
        }
        public async Task Delete(string id)
        {
            try
            {
                await _unitOfWork.SubMenu.Delete(Guid.Parse(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(SubMenuBusiness));
            }
        }
    }
}
