using IdentityApplication.Business.Contracts;
using IdentityApplication.Core;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Controllers
{
    [Authorize(Roles = $"{Constants.Roles.Administrator}")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentBusiness _business;
        public DepartmentController(IDepartmentBusiness business)
        {
            _business = business;
        }

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

        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        [HttpPost]
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

        [HttpGet]
        public IActionResult GetCategories(string id)
        {
            try
            {
                var response = _business.GetDepartmentById(id);

                var sub = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "All" }
                };

                foreach (var item in response.Categories.ToList())
                {
                    sub.Add(new SelectListItem { Value = item.CategoryId.ToString(), Text = item.CategoryName });
                }

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
