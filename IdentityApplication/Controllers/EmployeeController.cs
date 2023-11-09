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
        public async Task<IActionResult> Create(InsertEmployeeRequest employee)
        {

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var locations = _unitOfWork.Location.GetLocations();
            var departments = _unitOfWork.Department.GetDepartments();
            var categories = _unitOfWork.Category.GetCategories();
            var subCategories = _unitOfWork.SubCategory.GetSubCategories();

            var ss = User.IsInRole(Constants.Roles.Administrator);

            if (user?.LocationId != Guid.Empty && !User.IsInRole(Constants.Roles.Administrator))
            {
                var itemLocation = locations.Where(e => e.Id == user?.LocationId);
                var locationItems = itemLocation.Select(location =>
                    new SelectListItem(
                        location.Name,
                        location.Id.ToString(),
                        false)).ToList();
                employee.SelectedLocation = user.LocationId.ToString();
                employee.Locations = locationItems;
            }
            else
            {
                var locationItems = locations.Select(location =>
                    new SelectListItem(
                        location.Name,
                        location.Id.ToString(),
                        false)).ToList();
                employee.Locations = locationItems;
            }

            if (!string.IsNullOrEmpty(employee.SelectedDepartment) && !User.IsInRole(Constants.Roles.Administrator))
            {
                var itemDepartments = departments.Where(e => e.Id.ToString() == employee.SelectedDepartment);
                var departmentItems = itemDepartments.Select(department =>
                    new SelectListItem(
                        department.Name,
                        department.Id.ToString(), false)).ToList();
                employee.Departments = departmentItems;
            }
            else
            {
                var departmentItems = departments.Select(department =>
                    new SelectListItem(
                        department.Name,
                        department.Id.ToString(), false)).ToList();
                employee.Departments = departmentItems;
            }

            if (!string.IsNullOrEmpty(employee.SelectedCategory) && !User.IsInRole(Constants.Roles.Administrator))
            {
                var itemCategories = categories.Where(e => e.Id.ToString() == employee.SelectedCategory);
                var categoryItems = itemCategories.Select(category =>
                    new SelectListItem(
                        category.Name,
                        category.Id.ToString(), false)).ToList();
                employee.Categories = categoryItems;
            }
            else
            {
                var categoryItems = categories.Select(category =>
                    new SelectListItem(
                        category.Name,
                        category.Id.ToString(), false)).ToList();
                employee.Categories = categoryItems;
            }

            if (!string.IsNullOrEmpty(employee.SelectedSubCategory) && !User.IsInRole(Constants.Roles.Administrator))
            {
                var itemSubCategories = subCategories.Where(e => e.Id.ToString() == employee.SelectedSubCategory);
                var subCategoryItems = itemSubCategories.Select(subCategory =>
                    new SelectListItem(
                        subCategory.Name,
                        subCategory.Id.ToString(), false)).ToList();
                employee.SubCategories = subCategoryItems;
            }
            else
            {
                var subCategoryItems = subCategories.Select(subCategory =>
                    new SelectListItem(
                        subCategory.Name,
                        subCategory.Id.ToString(), false)).ToList();
                employee.SubCategories = subCategoryItems;
            }

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
        public IActionResult List()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] PaginationFilter filter)
        {
            var response = await _business.GetAll(filter);
            return Json(new
            {
                draw = filter.draw,
                recordsTotal = response.TotalCount,
                recordsFiltered = response.TotalCount, // Use the total count as recordsFiltered
                data = response.Data.Select(e => _mapper.Map<ViewEmployeeModel>(e))
            });
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

            entity.SelectedCategory = category.Id.ToString();
            entity.SelectedSubCategory = subCategory.Id.ToString();
            entity.SelectedDepartment = department.Id.ToString();
            entity.SelectedLocation = location.Id.ToString();

            return RedirectToAction("Create", entity);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(InsertEmployeeRequest request)
        {
            try
            {
                _business.Update(request);

                TempData["SuccessMessage"] = "Employee updated successfully.";
                return RedirectToAction("Create", request);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Employee update failed.";
                return RedirectToAction("Create", request);
            }
        }

        [Authorize(Policy = $"{Constants.Policies.RequireAdmin}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _business.Delete(id);

            return Ok(id);
        }
    }
}
