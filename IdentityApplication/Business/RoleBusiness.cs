using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace IdentityApplication.Business
{
    public class RoleBusiness : IRoleBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RoleBusiness> _logger;

        public RoleBusiness(IUnitOfWork unitofwork, ILogger<RoleBusiness> logger)
        {
            _unitOfWork = unitofwork;
            _logger = logger;
        }

        public async Task Delete(string id)
        {
            try
            {
                await _unitOfWork.Role.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(RoleBusiness));
                throw ex;
            }
        }

        public async Task Create(string roleName)
        {
            try
            {
                await _unitOfWork.Role.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(RoleBusiness));
                throw ex;
            }
        }

        public async Task Update(RolesViewModel role)
        {
            try
            {
                var entity = new IdentityRole() { Id = role.RoleId, Name = role.RoleName.Trim() };
                await _unitOfWork.Role.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(RoleBusiness));
                throw ex;
            }
        }

        public async Task<List<IdentityRole>> GetAll()
        {
            var response = new List<IdentityRole>();
            try
            {
                response = await _unitOfWork.Role.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(RoleBusiness));
                throw ex;
            }
            return response;
        }
    }
}
