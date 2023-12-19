using AutoMapper;
using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace IdentityApplication.Business
{
    public class MenuBusiness : IMenuBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<MenuBusiness> _logger;
        public MenuBusiness(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, ILogger<MenuBusiness> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<List<MenuViewModel>> GetMenus()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if(user is null)
            {
                return null;
            }
            var isDev = await _userManager.IsInRoleAsync(user, Constants.Roles.SuperDeveloper);
            if (isDev)
            {
                var menus = _unitOfWork.Menu.GetMenus();
                return _mapper.Map<List<MenuViewModel>>(menus);
            }
            else
            {
                var role = await _userManager.GetRolesAsync(user);
                var roles = _unitOfWork.Role.GetRoles();
                var currentRole = roles.Where(ur => ur.Name == role.FirstOrDefault()).FirstOrDefault();

                var rolemenus = _unitOfWork.Menu.GetMenuById(currentRole.Id);
                return _mapper.Map<List<MenuViewModel>>(rolemenus);
            }
        }

        public List<Menu> GetAll()
        {
            var response = new List<Menu>();
            try
            {
                response = _unitOfWork.Menu.GetMenus();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(MenuBusiness));
            }
            return response;
        }

        public void Create(CreateMenuRequest request)
        {
            try
            {
                var entity = _mapper.Map<Menu>(request);
                _unitOfWork.Menu.Create(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(MenuBusiness));
            }
        }

        public PaginationResponse<MenuViewModel> GetMenusWithFilters(PaginationFilter filter)
        {
            var response = new PaginationResponse<MenuViewModel>();
            try
            {
                response = _unitOfWork.Menu.GetMenusWithFilters(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(MenuBusiness));
            }
            return response;
        }

        public void Update(CreateMenuRequest request)
        {
            try
            {
                var entity = _mapper.Map<Menu>(request);
                _unitOfWork.Menu.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(MenuBusiness));
            }
        }

        public async Task Delete(string id)
        {
            try
            {
                await _unitOfWork.Menu.Delete(Guid.Parse(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(MenuBusiness));
            }
        }
    }
}
