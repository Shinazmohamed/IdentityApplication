using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Controllers
{
    [Authorize]
    public class CategorySubCategoryMappingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategorySubCategoryBusiness _business;
        private readonly ILogger<CategorySubCategoryMappingController> _logger;

        public CategorySubCategoryMappingController(IUnitOfWork unitOfWork, ICategorySubCategoryBusiness business, ILogger<CategorySubCategoryMappingController> logger)
        {
            _unitOfWork = unitOfWork;
            _business = business;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var response = new CreateCategorySubCategoryRequest();

            try
            {
                var categories = _unitOfWork.Category.GetCategories();
                var subCategories = _unitOfWork.SubCategory.GetSubCategories();

                if (categories != null)
                {
                    response.Categories = categories.Select(category =>
                        new SelectListItem(category.CategoryName, category.CategoryId.ToString(), false)).ToList();
                }
                if (subCategories != null)
                {
                    response.SubCategories = subCategories.Select(subCategory =>
                        new SelectListItem(subCategory?.SubCategoryName, subCategory?.SubCategoryId.ToString(), false)).ToList();
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
            var response = new PaginationResponse<ListCategorySubCategoryModel>();
            try
            {
                response = await _business.GetAll(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Controller} All function error", typeof(CategoryDepartmentMappingController));
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
        public async Task<IActionResult> Create(CreateCategorySubCategoryRequest request)
        {
            try
            {
                await _business.CreateMapping(request);
                TempData["SuccessMessage"] = "Sub Category Mapped Successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Sub Category Mapping Failed.";
                _logger.LogError(ex, "{Controller} All function error", typeof(CategoryDepartmentMappingController));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(CreateCategorySubCategoryRequest request)
        {
            try
            {
                await _business.UpdateMapping(request);
                TempData["SuccessMessage"] = "Sub category mapping updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Sub category mapping update failed.";
                _logger.LogError(ex, "{Controller} All function error", typeof(CategoryDepartmentMappingController));
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string mappingId)
        {
            try
            {
                await _business.DeleteMapping(mappingId);
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
