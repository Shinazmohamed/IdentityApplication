using AutoMapper;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApplication.Controllers
{
    //[Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.User}")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeBusiness _business;
        private readonly IMapper _mapper;
        public EmployeeController(IEmployeeBusiness business, IMapper mapper)
        {
            _business = business;
            _mapper = mapper;
        }

        //[Authorize(Policy = $"{Constants.Policies.RequireAdmin}")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create(InsertEmployeeRequest employee)
        {
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(InsertEmployeeRequest model)
        {
            try
            {
                var _entity = _mapper.Map<Employee>(model);
                _business.Create(_entity);
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

            // Return the data in the format expected by DataTables
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
        public async Task<IActionResult> Edit(InsertEmployeeRequest model)
        {
            try
            {
                var _entity = _mapper.Map<Employee>(model);
                _business.Update(_entity);

                TempData["SuccessMessage"] = "Employee updated successfully.";
                return RedirectToAction("Create", model);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Employee update failed.";
                return RedirectToAction("Create", model);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _business.Delete(id);

            return Ok(id);
        }
    }
}
