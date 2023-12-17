using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace IdentityApplication.Business
{
    public class RoleBusiness : IRoleBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleBusiness(IUnitOfWork unitofwork)
        {
            _unitOfWork = unitofwork;
        }

        public async Task Delete(string id)
        {
            await _unitOfWork.Role.Delete(id);
        }

        public async Task Create(string roleName)
        {
            await _unitOfWork.Role.CreateAsync(new IdentityRole(roleName.Trim()));
        }

        public async Task Update(RolesViewModel role)
        {
            var entity = new IdentityRole() { Id = role.RoleId, Name = role.RoleName.Trim() };
            await _unitOfWork.Role.UpdateAsync(entity);
        }

        public async Task<List<IdentityRole>> GetAll()
        {
            return await _unitOfWork.Role.GetAll();
        }
    }
}
