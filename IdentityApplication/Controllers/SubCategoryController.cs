using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.PermissionHelper;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Controllers
{
    [Authorize]
    public class SubCategoryController : Controller
    {
        private readonly ISubCategoryBusiness _business;

        public SubCategoryController(ISubCategoryBusiness business)
        {
            _business = business;
        }

        [Authorize(policy: $"{PermissionsModel.SubCategory.View}")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.SubCategory.View}")]
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
        [Authorize(policy: $"{PermissionsModel.SubCategory.Create}")]
        public async Task<IActionResult> Create(CreateSubCategoryRequest request)
        {
            try
            {
                await _business.Create(request);
                TempData["SuccessMessage"] = "Sub category created successfully.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Sub category created failed.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.SubCategory.Edit}")]
        public async Task<IActionResult> Update(CreateSubCategoryRequest request)
        {
            try
            {
                await _business.Update(request);
                TempData["SuccessMessage"] = "Sub category updated successfully.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Sub category update failed.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.SubCategory.Delete}")]
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

        [HttpGet]
        public IActionResult GetSubcategories(string id)
        {
            try
            {
                var response = _business.GetSubCategoriesByCategoryId(id);

                var sub = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "All" }
                };

                sub.AddRange(response.Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name.ToString()
                }));

                return Json(sub);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "No Records found.";
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
