using AspNetCoreHero.ToastNotification.Abstractions;
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
        private readonly INotyfService _notyf;

        public CategorySubCategoryMappingController(IUnitOfWork unitOfWork, ICategorySubCategoryBusiness business, ILogger<CategorySubCategoryMappingController> logger, INotyfService notyf)
        {
            _unitOfWork = unitOfWork;
            _business = business;
            _logger = logger;
            _notyf = notyf;
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
                _notyf.Error("Operation Failed. Please contact administrator");
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
                _notyf.Error("Operation Failed. Please contact administrator");
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
                _notyf.Success("Sub category mapping successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
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
                _notyf.Success("Sub category mapping updated successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
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
                _notyf.Success("Record delete successfully");
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
