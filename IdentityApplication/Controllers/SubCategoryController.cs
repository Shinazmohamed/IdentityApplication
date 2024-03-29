﻿using AspNetCoreHero.ToastNotification.Abstractions;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.PermissionHelper;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Controllers
{
    [Authorize]
    public class SubCategoryController : Controller
    {
        private readonly ISubCategoryBusiness _business;
        private readonly ILogger<SubCategoryController> _logger;
        private readonly INotyfService _notyf;

        public SubCategoryController(ISubCategoryBusiness business, ILogger<SubCategoryController> logger, INotyfService notyf)
        {
            _business = business;
            _logger = logger;
            _notyf = notyf;
        }

        [Authorize(policy: $"{PermissionsModel.SubCategoryPermission.View}")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.SubCategoryPermission.View}")]
        public async Task<IActionResult> GetList([FromBody] PaginationFilter filter)
        {
            var response = new PaginationResponse<ListSubCategoryModel>();
            try
            {
                response = await _business.GetAllWithFilters(filter);
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(SubCategoryController));
            }
            return Json(new
            {
                filter.draw,
                recordsTotal = response.TotalCount,
                recordsFiltered = response.TotalCount,
                data = response.Data
            });
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.SubCategoryPermission.Create}")]
        public IActionResult Create(CreateSubCategoryRequest request)
        {
            try
            {
                _business.Create(request);
                _notyf.Success("Record created successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(SubCategoryController));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.SubCategoryPermission.Edit}")]
        public async Task<IActionResult> Update(CreateSubCategoryRequest request)
        {
            try
            {
                await _business.Update(request);
                _notyf.Success("Record updated successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(SubCategoryController));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.SubCategoryPermission.Delete}")]
        public async Task<IActionResult> Delete(string mappingId)
        {
            try
            {
                await _business.Delete(mappingId);
                _notyf.Success("Record deleted successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(SubCategoryController));
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetSubcategories(string id)
        {
            var response = new List<SelectListItem>();
            try
            {
                var subCategories = _business.GetSubCategoriesByCategoryId(id);
                response =
                [
                    new SelectListItem { Value = "", Text = "All" },
                    .. subCategories.Select(item => new SelectListItem
                    {
                        Value = item.Id.ToString(),
                        Text = item.Name.ToString()
                    }),
                ];
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(SubCategoryController));
            }

            return Json(response);
        }

    }
}
