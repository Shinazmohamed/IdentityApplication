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
        private readonly ILogger<CategoryDepartmentMappingController> _logger;

        public CategoryDepartmentMappingController(ICategoryDepartmentMappingBusiness business, IUnitOfWork unitOfWork, ILogger<CategoryDepartmentMappingController> logger)
        {
            _business = business;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var response = new CreateCategoryDepartmentMappingViewModel();
            try
            {
                var categories = _unitOfWork.Category.GetCategories();
                var departments = _unitOfWork.Department.GetDepartments();
                if (categories != null)
                {
                    response.Departments = departments.Select(department =>
                        new SelectListItem(department.DepartmentName, department.DepartmentId.ToString(), false)).ToList();
                }
                if (departments != null)
                {
                    response.Categories = categories.Select(category =>
                        new SelectListItem(category?.CategoryName, category?.CategoryId.ToString(), false)).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Controller} All function error", typeof(CategoryDepartmentMappingController));
            }
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] PaginationFilter filter)
        {
            var response = new PaginationResponse<ListCategoryDepartmentMappingViewModel>();

            try
            {
                response = await _business.GetAllWithFilters(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Controller} All function error", typeof(CategoryDepartmentMappingController));
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
        public async Task<IActionResult> Create(CreateCategoryDepartmentMappingViewModel request)
        {
            try
            {
                await _business.CreateMapping(request);
                TempData["SuccessMessage"] = "Category Mapped Successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Category Mapping Failed.";
                _logger.LogError(ex, "{Controller} All function error", typeof(CategoryDepartmentMappingController));
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Category mapping update failed.";
                _logger.LogError(ex, "{Controller} All function error", typeof(CategoryDepartmentMappingController));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(string mappingId)
        {
            try
            {
                _business.DeleteMapping(mappingId);
                TempData["SuccessMessage"] = "Record deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Record delete failed.";
                _logger.LogError(ex, "{Controller} All function error", typeof(CategoryDepartmentMappingController));
            }
            return RedirectToAction("Index");
        }
    }
}
