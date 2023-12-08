using AutoMapper;
using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Controllers
{
    [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.User}")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeBusiness _business;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeeController(IEmployeeBusiness business, IMapper mapper, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _business = business;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var employee = new InsertEmployeeRequest();
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var locations = _unitOfWork.Location.GetLocations();
            var departments = _unitOfWork.Department.GetDepartments();
            var categories = _unitOfWork.Category.GetCategories();
            var isAdmin = User.IsInRole(Constants.Roles.Administrator);

            if (user?.LocationId != Guid.Empty && !isAdmin)
            {
                var itemLocation = locations.FirstOrDefault(e => e.LocationId == user?.LocationId);
                employee.SelectedLocation = user?.LocationId.ToString();
                employee.Locations = new List<SelectListItem>
                {
                    new SelectListItem(itemLocation?.LocationName, itemLocation?.LocationId.ToString(), false)
                };
            }
            else
            {
                employee.Locations = locations.Select(location =>
                    new SelectListItem(location.LocationName, location.LocationId.ToString(), false)).ToList();
            }

            employee.Departments = departments.Select(department =>
                new SelectListItem(department.DepartmentName, department.DepartmentId.ToString(), false)).ToList();

            employee.Categories = categories.Select(category =>
                new SelectListItem(category.CategoryName, category.CategoryId.ToString(), false)).ToList();

            employee.SubCategories = new List<SelectListItem>();

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(InsertEmployeeRequest model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.SelectedLocation))
                {
                    var locations = _unitOfWork.Location.GetLocations();
                    var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                    model.SelectedLocation = user.LocationId.ToString();
                }

                _business.Create(model);
                TempData["SuccessMessage"] = "Employee created successfully.";
                return RedirectToAction("Create");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Employee creation failed.";
                return View("Create", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var response = new InsertEmployeeRequest();
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var isAdmin = User.IsInRole(Constants.Roles.Administrator);

            var locations = _unitOfWork.Location.GetLocations();
            if (!isAdmin)
            {
                var itemLocation = locations.FirstOrDefault(e => e.LocationId == user?.LocationId);
                response.SelectedLocation = user?.LocationId.ToString();
                response.Locations = new List<SelectListItem>
                {
                    new SelectListItem(itemLocation?.LocationName, itemLocation?.LocationId.ToString(), false)
                };
            }
            else
            {
                response.Locations = locations.Select(location =>
                    new SelectListItem(location.LocationName, location.LocationId.ToString(), false)).ToList();
            }


            var departments = _unitOfWork.Department.GetDepartments();
            response.Departments = departments.Select(department =>
                    new SelectListItem(department.DepartmentName, department.DepartmentId.ToString(), false)).ToList();


            //var categories = _unitOfWork.Category.GetCategories();
            //response.Categories = categories.Select(category =>
            //    new SelectListItem(category.CategoryName, category.CategoryId.ToString(), false)).ToList();

            //var subCategories = _unitOfWork.SubCategory.GetSubCategories();
            //response.SubCategories = subCategories.Select(subCategory =>
            //    new SelectListItem(subCategory?.SubCategoryName, subCategory?.SubCategoryId.ToString(), false)).ToList();

            return View(response);
        }

        [HttpPost]

        public async Task<IActionResult> GetList([FromBody] PaginationFilter filter)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (!User.IsInRole(Constants.Roles.Administrator)) filter.location = user.LocationId.ToString();

            var response = await _business.GetAll(filter);
            var dataSrc = new
            {
                draw = filter.draw,
                recordsTotal = response.TotalCount,
                recordsFiltered = response.TotalCount, // Use the total count as recordsFiltered
                data = response.Data.Select(e => _mapper.Map<ViewEmployeeModel>(e))
            };
            return Json(dataSrc);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var currentData = await _business.GetById(id);
            var entity = _mapper.Map<InsertEmployeeRequest>(currentData);

            var category = _unitOfWork.Category.GetCategoryByName(currentData.CategoryName);
            var subCategory = _unitOfWork.SubCategory.GetSubCategoryByName(currentData.SubCategoryName);
            var location = _unitOfWork.Location.GetLocationByName(currentData.LocationName);
            var department = _unitOfWork.Department.GetDepartmentByName(currentData.DepartmentName);

            entity.SelectedCategory = category.CategoryId.ToString();
            entity.SelectedSubCategory = subCategory.SubCategoryId.ToString();
            entity.SelectedDepartment = department.DepartmentId.ToString();
            entity.SelectedLocation = location.LocationId.ToString();

            return Json(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(InsertEmployeeRequest request)
        {
            try
            {
                await _business.Update(request, User.IsInRole(Constants.Roles.Administrator));

                TempData["SuccessMessage"] = "Record updated successfully.";
                return RedirectToAction("List");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Record update failed.";
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string Id)
        {
            try
            {
                if (User.HasClaim("Permission", "RequireAdmin"))
                {
                    await _business.Delete(Id);

                    TempData["SuccessMessage"] = "Record deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Authorization error: You do not have permission to perform this action.";
                }

                return RedirectToAction("List");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Record delete failed.";
                return RedirectToAction("List");
            }
        }
    }
}
