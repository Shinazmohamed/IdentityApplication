using IdentityApplication.Business.Contracts;
using IdentityApplication.Core;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Controllers
{
    [Authorize(Roles = $"{Constants.Roles.Administrator}")]
    public class CategoryController : Controller
    {
        private readonly ICategoryBusiness _business;
        public CategoryController(ICategoryBusiness business)
        {
            _business = business;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetSubcategories(string id) 
        {
            try
            {
                var response = _business.GetCategoryById(id);

                var sub = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "All" }
                };

                foreach (var item in response.SubCategories.ToList())
                {
                    sub.Add(new SelectListItem { Value = item.SubCategoryId.ToString(), Text = item.SubCategoryName });
                }

                return Json(sub);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "No Records found.";
                return StatusCode(500, "Internal Server Error");
            }
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
        public async Task<IActionResult> Create(CreateCategoryRequest request)
        {
            try
            {
                await _business.Create(request);
                TempData["SuccessMessage"] = "Category created successfully.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Category created failed.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(CreateCategoryRequest request)
        {
            try
            {
                await _business.Update(request);
                TempData["SuccessMessage"] = "Category updated successfully.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Category update failed.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
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
