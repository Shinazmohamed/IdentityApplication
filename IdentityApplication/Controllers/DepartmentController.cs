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
        public DepartmentController(IDepartmentBusiness business)
        {
            _business = business;
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
            var response = await _business.GetAllWithFilters(filter);
            var jsonD = new
            {
                filter.draw,
                recordsTotal = response.TotalCount,
                recordsFiltered = response.TotalCount,
                data = response.Data
            };
            return Json(jsonD);
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
            catch
            {
                TempData["ErrorMessage"] = "Department created failed.";
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
            catch
            {
                TempData["ErrorMessage"] = "Department update failed.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.DepartmentPermission.Delete}")]
        public async Task<IActionResult> Delete(string mappingId)
        {
            try
            {
                if (User.HasClaim("Permission", "RequireAdmin"))
                {
                    await _business.Delete(mappingId);

                    TempData["SuccessMessage"] = "Record deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Authorization error: You do not have permission to perform this action.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Record delete failed.";
                return RedirectToAction("Index");
            }
        }
    }
}
