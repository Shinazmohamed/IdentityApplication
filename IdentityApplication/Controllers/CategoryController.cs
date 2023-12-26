using AspNetCoreHero.ToastNotification.Abstractions;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.PermissionHelper;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryBusiness _business;
        private readonly ILogger<CategoryController> _logger;
        private readonly INotyfService _notyf;
        public CategoryController(ICategoryBusiness business, ILogger<CategoryController> logger, INotyfService notyf)
        {
            _business = business;
            _logger = logger;
            _notyf = notyf;
        }

        [Authorize(policy: $"{PermissionsModel.CategoryPermission.Create}")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetCategoryByDepartmentId(string id)
        {
            var response = new List<SelectListItem>();
            try
            {
                var categories = _business.GetCategoryByDepartmentId(id);
                if (categories != null)
                {
                    response =
                    [
                        new SelectListItem { Value = "", Text = "All" },
                        .. categories.Select(item => new SelectListItem
                        {
                            Value = item.Id.ToString(),
                            Text = item.Name
                        }),
                    ];
                }
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(CategoryController));
            }
            return Json(response);
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.CategoryPermission.View}")]
        public async Task<IActionResult> GetList([FromBody] PaginationFilter filter)
        {
            var response = new PaginationResponse<ListCategoryModel>();
            try
            {
                response = await _business.GetAllWithFilters(filter);
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(CategoryController));
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
        [Authorize(policy: $"{PermissionsModel.CategoryPermission.Create}")]
        public async Task<IActionResult> Create(CreateCategoryRequest request)
        {
            try
            {
                await _business.Create(request);
                _notyf.Success("Category created successfull");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(CategoryController));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.CategoryPermission.Edit}")]
        public async Task<IActionResult> Update(CreateCategoryRequest request)
        {
            try
            {
                await _business.Update(request);
                _notyf.Success("Category updated successfull");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(CategoryController));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.CategoryPermission.Delete}")]
        public async Task<IActionResult> Delete(string mappingId)
        {
            try
            {
                await _business.Delete(mappingId);
                _notyf.Success("Category deleted successfully");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(CategoryController));
            }

            return RedirectToAction("Index");
        }

    }
}
