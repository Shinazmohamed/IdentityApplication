using AspNetCoreHero.ToastNotification.Abstractions;
using Azure;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.PermissionHelper;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Controllers
{
    [Authorize]
    public class StaffController : Controller
    {
        private readonly ILogger<StaffController> _logger;
        private readonly INotyfService _notyf;
        private readonly IStaffBusiness _business;
        private readonly ITeamBusiness _teamBusiness;

        public StaffController(ILogger<StaffController> logger, INotyfService notyf, IStaffBusiness business, ITeamBusiness teamBusiness)
        {
            _logger = logger;
            _notyf = notyf;
            _business = business;
            _teamBusiness = teamBusiness;
        }

        [Authorize(policy: $"{PermissionsModel.StaffPermission.View}")]
        public IActionResult Index()
        {
            var response = new CreateStaffRequest();
            
            response.StaffTeamCollection = GetTeamsSelectCollection();

            return View(response);
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(policy: $"{PermissionsModel.StaffPermission.Create}")]
        public async Task<IActionResult> CreateAsync(CreateStaffRequest request)
        {
            var response = new CreateStaffRequest();
            try
            {
                await _business.CreateAsync(request);
                _notyf.Success("Record created successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(StaffController));
            }


            response.StaffTeamCollection = GetTeamsSelectCollection();
            return View("Index", response);
        }

        [HttpPost]
        public async Task<IActionResult> GetStaffByFilters([FromBody] PaginationFilter filter)
        {
            var response = new PaginationResponse<ViewStaffResponse>();
            try
            {
                response = await _business.GetAll(filter);
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(StaffController));
            }
            return Json(new
            {
                filter.draw,
                recordsTotal = response.TotalCount,
                recordsFiltered = response.TotalCount,
                data = response.Data
            });
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.StaffPermission.Delete}")]
        public IActionResult Delete(string staffId)
        {
            var response = new CreateStaffRequest();
            try
            {
                _business.Delete(staffId);
                _notyf.Success("Record deleted successfully");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(StaffController));
            }

            response.StaffTeamCollection = GetTeamsSelectCollection();
            return RedirectToAction("Index", response);
        }

        private List<SelectListItem> GetTeamsSelectCollection()
        {
            var teams = _teamBusiness.GetAll();

            return teams.Select(team =>
                new SelectListItem(team.TeamName, team.TeamId.ToString(), false)).ToList();
        }
    }
}
