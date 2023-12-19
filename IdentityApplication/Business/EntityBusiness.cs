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
        private readonly ILogger<EntityBusiness> _logger;

        public EntityBusiness(IUnitOfWork unitofwork, IMapper mapper, ILogger<EntityBusiness> logger)
        {
            _unitOfWork = unitofwork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Create(ManagePermission request)
        {
            try
            {
                var entity = _mapper.Map<Entity>(request);
                _unitOfWork.Entity.Create(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(EntityBusiness));
            }
        }

        public async Task Edit(ManagePermission request)
        {
            try
            {
                var entity = _mapper.Map<Entity>(request);
                _unitOfWork.Entity.Edit(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(EntityBusiness));
            }
        }

        public IList<Entity> GetEntities()
        {
            var response = new List<Entity>();
            try
            {
                response = _unitOfWork.Entity.GetEntities();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(EntityBusiness));
            }
            return response;
        }

        public async Task Delete(string id)
        {
            try
            {
                await _unitOfWork.Entity.Delete(Guid.Parse(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(EntityBusiness));
            }
        }
    }
}
