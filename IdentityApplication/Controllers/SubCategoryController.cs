using AutoMapper;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Controllers
{
    public class SubCategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubCategoryBusiness _business;
        private readonly IMapper _mapper;
        public SubCategoryController(IUnitOfWork unitOfWork, ISubCategoryBusiness business, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _business = business;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var response = new CreateSubCategoryRequest();

            var categories = _unitOfWork.Category.GetCategories();
            var subCategories = _unitOfWork.SubCategory.GetSubCategories();

            response.Categories = categories.Select(category =>
                new SelectListItem(category.CategoryName, category.CategoryId.ToString(), false)).ToList();

            response.SubCategories = subCategories.Select(subCategory =>
                new SelectListItem(subCategory?.SubCategoryName, subCategory?.SubCategoryId.ToString(), false)).ToList();

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] PaginationFilter filter)
        {
            var response = await _business.GetAll(filter);
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
        public async Task<IActionResult> Create(CreateSubCategoryRequest request)
        {
            try
            {
                await _business.CreateMapping(request);
                TempData["SuccessMessage"] = "Sub Category Mapped Successfully.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Sub Category Mapping Failed.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(CreateSubCategoryRequest request)
        {
            try
            {
                await _business.UpdateMapping(request);
                TempData["SuccessMessage"] = "Sub category mapping updated successfully.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Sub category mapping update failed.";
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
                    await _business.DeleteMapping(mappingId);

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
