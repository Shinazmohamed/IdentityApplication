using AutoMapper;
using Azure.Core;
using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace IdentityApplication.Business
{
    public class StaffBusiness : IStaffBusiness
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<StaffBusiness> _logger;

        public StaffBusiness(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper, ILogger<StaffBusiness> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateAsync(CreateStaffRequest request)
        {
            try
            {
                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                var entity = _mapper.Map<Staff>(request);
                entity.LocationId = Guid.Parse(user.LocationId.ToString());

                _unitOfWork.Staff.Create(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(StaffBusiness));
                throw ex;
            }
        }

        public async Task<PaginationResponse<ViewStaffResponse>> GetAll(PaginationFilter filter)
        {
            var response = new PaginationResponse<ViewStaffResponse>();
            try
            {

                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                filter.location = user.LocationId.ToString();

                response = await _unitOfWork.Staff.GetEntitiesWithFilters(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(StaffBusiness));
                throw ex;
            }
            return response;
        }

        public void Delete(string staffId)
        {
            try
            {
                _unitOfWork.Staff.Delete(Guid.Parse(staffId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(StaffBusiness));
                throw ex;
            }
        }
    }
}
