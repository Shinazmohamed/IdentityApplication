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

            var locations = _unitOfWork.Location.GetLocations();
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            var locationItems = locations.Select(location =>
                new SelectListItem(
                    location.Name,
                    location.Id.ToString(), false)).ToList();
            employee.SelectedLocation = user.LocationId.ToString();
            employee.Locations = locationItems;


            var departments = _unitOfWork.Department.GetDepartments();
            var departmentItems = departments.Select(department =>
                new SelectListItem(
                    department.Name,
                    department.Id.ToString(), false)).ToList();
            employee.Departments = departmentItems;

            var categories = _unitOfWork.Category.GetCategories();
            var categoryItems = categories.Select(category =>
                new SelectListItem(
                    category.Name,
                    category.Id.ToString(), false)).ToList();
            employee.Categories = categoryItems;

            var subCategories = _unitOfWork.SubCategory.GetSubCategories();
            var subCategoryItems = subCategories.Select(subCategory =>
                new SelectListItem(
                    subCategory.Name,
                    subCategory.Id.ToString(), false)).ToList();
            employee.SubCategories = subCategoryItems;

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(InsertEmployeeRequest model)
        {
            try
            {
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
            var _entity = _mapper.Map<InsertEmployeeRequest>(await _business.GetById(id));
            return RedirectToAction("Create", _entity);
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
