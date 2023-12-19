using AutoMapper;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business
{
    public class PermissionBusiness : IPermissionBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PermissionBusiness> _logger;

        public PermissionBusiness(IUnitOfWork unitofwork, IMapper mapper, ILogger<PermissionBusiness> logger)
        {
            _unitOfWork = unitofwork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaginationResponse<Entity>> GetEntitiesWithFilters(PaginationFilter filter)
        {
            var response = new PaginationResponse<Entity>();
            try
            {
                response = await _unitOfWork.Permission.GetEntitiesWithFilters(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(MenuBusiness));
            }
            return response;
        }
        public async Task<PaginationResponse<Permission>> GetPermissionsWithFilters(PaginationFilter filter)
        {
            var response = new PaginationResponse<Permission>();
            try
            {
                response = await _unitOfWork.Permission.GetPermissionsWithFilters(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(MenuBusiness));
            }
            return response;
        }

        public async Task Create(ManagePermission request)
        {
            try
            {
                var entity = _mapper.Map<Permission>(request);
                _unitOfWork.Permission.Create(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(MenuBusiness));
            }
        }

        public async Task Update(ManagePermission request)
        {
            try
            {
                var entity = _mapper.Map<Permission>(request);
                _unitOfWork.Permission.Update(entity);
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
                await _unitOfWork.Permission.Delete(Guid.Parse(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(MenuBusiness));
            }
        }
    }
}
