using IdentityApplication.Business.Contracts;
using IdentityApplication.Core;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApplication.Controllers
{
    [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.User}")]
    public class SubCategoryController : Controller
    {
        private readonly ISubCategoryBusiness _business;

        public SubCategoryController(ISubCategoryBusiness business)
        {
            _business = business;
        }

        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public IActionResult Index()
        {
            return View();
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

        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        [HttpPost]
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

        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
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
