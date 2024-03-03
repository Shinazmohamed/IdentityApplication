using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Azure;
using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.PermissionHelper;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeBusiness _business;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<EmployeeController> _logger;
        private readonly INotyfService _notyf;

        public EmployeeController(IEmployeeBusiness business, IMapper mapper, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService, ILogger<EmployeeController> logger, INotyfService notyf)
        {
            _business = business;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _authorizationService = authorizationService;
            _logger = logger;
            _notyf = notyf;
        }

        [HttpGet]
        [Authorize(policy: $"{PermissionsModel.EmployeePermission.Create}")]
        public async Task<IActionResult> Create()
        {
            var response = new InsertEmployeeRequest();
            try
            {
                response.SubCategories = new List<SelectListItem>();
                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                var locations = _unitOfWork.Location.GetLocations();
                var departments = _unitOfWork.Department.GetDepartments();
                var categories = _unitOfWork.Category.GetCategories();

                if (user?.LocationId != Guid.Empty && !isAdminOrSuperDev())
                {
                    if (locations.Any())
                    {
                        var itemLocation = locations.FirstOrDefault(e => e.LocationId == user?.LocationId);
                        response.Locations = new List<SelectListItem>
                    {
                        new SelectListItem(itemLocation?.LocationName, itemLocation?.LocationId.ToString(), false)
                    };
                    }
                    response.SelectedLocation = user?.LocationId.ToString();
                }
                else
                {
                    if (locations.Any())
                    {
                        response.Locations = locations.Select(location =>
                            new SelectListItem(location.LocationName, location.LocationId.ToString(), false)).ToList();
                    }
                }

                if (departments.Any())
                {
                    response.Departments = departments.Select(department =>
                        new SelectListItem(department.DepartmentName, department.DepartmentId.ToString(), false)).ToList();
                }
                if (categories.Any())
                {
                    response.Categories = categories.Select(category =>
                        new SelectListItem(category.CategoryName, category.CategoryId.ToString(), false)).ToList();
                }
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(EmployeeController));
            }

            return View(response);
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.EmployeePermission.Create}")]
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
                _notyf.Success("Record created successfully.");

                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(EmployeeController));
            }
            return View("Create", model);
        }

        [HttpGet]
        [Authorize(policy: $"{PermissionsModel.EmployeePermission.View}")]
        public async Task<IActionResult> List()
        {
            var response = new ListEmployeeRequest();

            try
            {
                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                var _isadminordev = isAdminOrSuperDev();

                var edit = await _authorizationService.AuthorizeAsync(User, PermissionsModel.EmployeePermission.Edit);
                response.Edit = edit.Succeeded;
                var delete = await _authorizationService.AuthorizeAsync(User, PermissionsModel.EmployeePermission.Delete);
                response.Delete = delete.Succeeded;
                response.IsAdminOrSuperDev = _isadminordev;

                var locations = _unitOfWork.Location.GetLocations();
                var departments = _unitOfWork.Department.GetDepartments();

                if (!_isadminordev)
                {
                    if (locations.Any())
                    {
                        var itemLocation = locations.FirstOrDefault(e => e.LocationId == user?.LocationId);
                        response.Locations = new List<SelectListItem>
                        {
                            new SelectListItem(itemLocation?.LocationName, itemLocation?.LocationId.ToString(), false)
                        };
                    }
                    response.SelectedLocation = user?.LocationId.ToString();
                }
                else
                {
                    if (locations.Any())
                    {
                        response.Locations = locations.Select(location =>
                        new SelectListItem(location.LocationName, location.LocationId.ToString(), false)).ToList();
                    }
                }
                if (departments.Any())
                {
                    response.Departments = departments.Select(department =>
                            new SelectListItem(department.DepartmentName, department.DepartmentId.ToString(), false)).ToList();
                }
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(EmployeeController));
            }

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] PaginationFilter filter)
        {
            var response = new PaginationResponse<Employee>();
            try
            {
                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                if (!isAdminOrSuperDev()) filter.location = user?.LocationId.ToString();

                response = await _business.GetAll(filter);
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(EmployeeController));
            }
            return Json(new
            {
                filter.draw,
                recordsTotal = response.TotalCount,
                recordsFiltered = response.TotalCount,
                data = response.Data.Select(e => _mapper.Map<ViewEmployeeModel>(e))
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id, string month)
        {
            var response = new ListEmployeeRequest();
            try
            {
                var entity = await _business.GetById(id, month);
                response = _mapper.Map<ListEmployeeRequest>(entity);

                var category = _unitOfWork.Category.GetCategoryByName(entity.CategoryName);
                var subCategory = _unitOfWork.SubCategory.GetSubCategoryByName(entity.SubCategoryName);
                var location = _unitOfWork.Location.GetLocationByName(entity.LocationName);
                var department = _unitOfWork.Department.GetDepartmentByName(entity.DepartmentName);

                if(category != null)
                    response.SelectedCategory = category.CategoryId.ToString();
                if(subCategory != null)
                    response.SelectedSubCategory = subCategory.SubCategoryId.ToString();
                if(department != null)
                    response.SelectedDepartment = department.DepartmentId.ToString();
                if(location != null)
                    response.SelectedLocation = location.LocationId.ToString();
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(EmployeeController));
            }
            return Json(response);
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.EmployeePermission.Edit}")]
        public async Task<IActionResult> Edit([FromBody] ListEmployeeRequest request)
        {
            var response = new ListEmployeeRequest();
            try
            {
                response = await _business.Update(request, isAdminOrSuperDev());

                _notyf.Success("Record updated successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(EmployeeController));
                return BadRequest(ex);
            }
            return Ok(response);
        }

        [HttpDelete]
        [Authorize(policy: $"{PermissionsModel.EmployeePermission.Delete}")]
        public async Task<IActionResult> Delete(string Id, string month)
        {
            try
            {
                await _business.Delete(Id, month);
                _notyf.Success("Record deleted successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(EmployeeController));
                return BadRequest(ex);
            }
            return Ok();
        }

        public bool isAdminOrSuperDev()
        {
            var isAdmin = User.IsInRole(Constants.Roles.Administrator);
            var isSuperDev = User.IsInRole(Constants.Roles.SuperDeveloper);
            return isAdmin || isSuperDev;
        }
    }
}
