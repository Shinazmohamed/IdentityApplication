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

        public PermissionBusiness(IUnitOfWork unitofwork, IMapper mapper)
        {
            _unitOfWork = unitofwork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<Entity>> GetEntitiesWithFilters(PaginationFilter filter)
        {
            return await _unitOfWork.Permission.GetEntitiesWithFilters(filter);
        }
        public async Task<PaginationResponse<Permission>> GetPermissionsWithFilters(PaginationFilter filter)
        {
            return await _unitOfWork.Permission.GetPermissionsWithFilters(filter);
        }

        public async Task Create(CreatePermission request)
        {
            var entity = _mapper.Map<Permission>(request);
            _unitOfWork.Permission.Create(entity);
        }

        public async Task Update(CreatePermission request)
        {
            var entity = _mapper.Map<Permission>(request);
            _unitOfWork.Permission.Update(entity);
        }

        public async Task Delete(string id)
        {
            await _unitOfWork.Permission.Delete(Guid.Parse(id));
        }
    }
}
