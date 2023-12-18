using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.PermissionHelper;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApplication.Controllers
{
    [Authorize]
    public class AuditController : Controller
    {
        private readonly IAuditBusiness _business;
        private readonly ILogger<AuditController> _logger;

        public AuditController(IAuditBusiness business, ILogger<AuditController> logger)
        {
            _business = business;
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.AuditPermission.View}")]
        public async Task<IActionResult> GetList([FromBody] PaginationFilter filter)
        {
            var response = new PaginationResponse<ListAuditModel>();
            try
            {
                response = await _business.GetAllWithFilters(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Controller} All function error", typeof(AuditController));
            }
            return Json(new
            {
                filter.draw,
                recordsTotal = response?.TotalCount,
                recordsFiltered = response?.TotalCount,
                data = response?.Data
            });
        }
    }
}
