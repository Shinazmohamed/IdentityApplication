using IdentityApplication.Business.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Controllers
{
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

                var sub = new List<SelectListItem>();

                sub.Add(new SelectListItem { Value = "", Text = "All" });

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

    }
}
