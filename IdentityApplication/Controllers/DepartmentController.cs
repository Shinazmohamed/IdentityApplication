using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.PermissionHelper;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApplication.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentBusiness _business;
        private readonly ILogger<DepartmentController> _logger;
        public DepartmentController(IDepartmentBusiness business, ILogger<DepartmentController> logger)
        {
            _business = business;
            _logger = logger;
        }

        [Authorize(policy: $"{PermissionsModel.DepartmentPermission.Create}")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.DepartmentPermission.View}")]
        public async Task<IActionResult> GetList([FromBody] PaginationFilter filter)
        {
            var response = new PaginationResponse<ListDepartmentViewModel>();
            try
            {
                response = await _business.GetAllWithFilters(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Controller} All function error", typeof(DepartmentController));
            }
            return Json(new
            {
                filter.draw,
                recordsTotal = response?.TotalCount,
                recordsFiltered = response?.TotalCount,
                data = response?.Data
            });
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.DepartmentPermission.Create}")]
        public async Task<IActionResult> Create(CreateDepartmentViewModel request)
        {
            try
            {
                await _business.Create(request);
                TempData["SuccessMessage"] = "Department created successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Department created failed.";
                _logger.LogError(ex, "{Controller} All function error", typeof(DepartmentController));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.DepartmentPermission.Edit}")]
        public async Task<IActionResult> Update(CreateDepartmentViewModel request)
        {
            try
            {
                await _business.Update(request);
                TempData["SuccessMessage"] = "Department updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Department update failed.";
                _logger.LogError(ex, "{Controller} All function error", typeof(DepartmentController));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.DepartmentPermission.Delete}")]
        public async Task<IActionResult> Delete(string mappingId)
        {
            try
            {
                await _business.Delete(mappingId);
                TempData["SuccessMessage"] = "Record deleted successfully.";
            }
            catch (Exception ex)
            { 
                TempData["ErrorMessage"] = "Record delete failed.";
                _logger.LogError(ex, "{Controller} All function error", typeof(DepartmentController));
            }
            return RedirectToAction("Index");
        }
    }
}
