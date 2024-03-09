using AspNetCoreHero.ToastNotification.Abstractions;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApplication.Controllers
{
    public class StaffController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly INotyfService _notyf;
        private readonly IStaffBusiness _business;

        public StaffController(ILogger<EmployeeController> logger, INotyfService notyf, IStaffBusiness business)
        {
            _logger = logger;
            _notyf = notyf;
            _business = business;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateAsync(CreateStaffRequest request)
        {
            try
            {
                await _business.CreateAsync(request);
                _notyf.Success("Record created successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(EmployeeController));
            }
            return View("Index");
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
                _logger.LogError(ex, "{Controller} All function error", typeof(EmployeeController));
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
        public IActionResult Delete(string staffId)
        {
            try
            {
                _business.Delete(staffId);
                _notyf.Success("Record deleted successfully");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(CategoryDepartmentMappingController));
            }
            return RedirectToAction("Index");
        }
    }
}
