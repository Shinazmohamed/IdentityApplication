using IdentityApplication.Business.Contracts;
using IdentityApplication.Core;
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
        public CategoryController(ICategoryBusiness business)
        {
            _business = business;
        }

        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetCategoryByDepartmentId(string id)
        {
            try
            {
                var response = _business.GetCategoryByDepartmentId(id);

                var sub = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "All" }
                };

                sub.AddRange(response.Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                }));

                return Json(sub);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "No Records found.";
                return StatusCode(500, "Internal Server Error");
            }
        }

        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
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

        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
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

        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
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

        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        [HttpPost]
        public async Task<IActionResult> Delete(string mappingId)
        {
            try
            {
                await _business.Delete(mappingId);

                TempData["SuccessMessage"] = "Record deleted successfully.";

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
