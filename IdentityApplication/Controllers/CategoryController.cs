using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryBusiness _business;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(ICategoryBusiness business, IUnitOfWork unitOfWork)
        {
            _business = business;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public IActionResult GetSubcategories(string id)
        //{
        //    try
        //    {
        //        return Ok(_business.GetCategoryById(id));
        //        //return Json(new { data = response.SubCategories });
        //    }
        //    catch (Exception)
        //    {
        //        TempData["ErrorMessage"] = "No Records found.";
        //        return NotFound();
        //    }
        //}

        [HttpGet]
        public IActionResult GetSubcategories(string id) 
        {
            try
            {
                var response = _business.GetCategoryById(id);

                var sub = new List<SelectListItem>();

                // Add the "All" option
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
