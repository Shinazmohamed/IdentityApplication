using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Controllers
{
    [Authorize]
    public class CategoryDepartmentMappingController : Controller
    {
        private readonly ICategoryDepartmentMappingBusiness _business;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryDepartmentMappingController(ICategoryDepartmentMappingBusiness business, IUnitOfWork unitOfWork)
        {
            _business = business;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var response = new CreateCategoryDepartmentMappingViewModel();

            var categories = _unitOfWork.Category.GetCategories();
            var departments = _unitOfWork.Department.GetDepartments();

            response.Departments = departments.Select(department =>
                new SelectListItem(department.DepartmentName, department.DepartmentId.ToString(), false)).ToList();

            response.Categories = categories.Select(category =>
                new SelectListItem(category?.CategoryName, category?.CategoryId.ToString(), false)).ToList();

            return View(response);
        }

        [HttpPost]
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
        public async Task<IActionResult> Create(CreateCategoryDepartmentMappingViewModel request)
        {
            try
            {
                await _business.CreateMapping(request);
                TempData["SuccessMessage"] = "Category Mapped Successfully.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Category Mapping Failed.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(CreateCategoryDepartmentMappingViewModel request)
        {
            try
            {
                await _business.UpdateMapping(request);
                TempData["SuccessMessage"] = "Category mapping updated successfully.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Category mapping update failed.";
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(string mappingId)
        {
            try
            {
                if (User.HasClaim("Permission", "RequireAdmin"))
                {
                    _business.DeleteMapping(mappingId);

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
