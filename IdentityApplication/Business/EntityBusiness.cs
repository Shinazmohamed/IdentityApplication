using AutoMapper;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business
{
    public class EntityBusiness : IEntityBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EntityBusiness(IUnitOfWork unitofwork, IMapper mapper)
        {
            _unitOfWork = unitofwork;
            _mapper = mapper;
        }

        public async Task Create(ManagePermission request)
        {
            var entity = _mapper.Map<Entity>(request);
            _unitOfWork.Entity.Create(entity);
        }

        public async Task Edit(ManagePermission request)
        {
            var entity = _mapper.Map<Entity>(request);
            _unitOfWork.Entity.Edit(entity);
        }

        public IList<Entity> GetEntities()
        {
            return _unitOfWork.Entity.GetEntities();
        }

        public async Task Delete(string id)
        {
            await _unitOfWork.Entity.Delete(Guid.Parse(id));
        }
    }
}
