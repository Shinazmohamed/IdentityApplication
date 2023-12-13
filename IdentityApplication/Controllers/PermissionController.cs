using AutoMapper;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Helpers;
using IdentityApplication.Core.PermissionHelper;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Controllers
{
    [Authorize]
    public class PermissionController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IPermissionBusiness _business;
        private readonly IEntityBusiness _entitybusiness;
        private readonly IMapper _mapper;
        public PermissionController(RoleManager<IdentityRole> roleManager, IAuthorizationService authorizationService, IPermissionBusiness business, IMapper mapper, IEntityBusiness entitybusiness)
        {
            _roleManager = roleManager;
            _authorizationService = authorizationService;
            _business = business;
            _mapper = mapper;
            _entitybusiness = entitybusiness;
        }

        [Authorize(policy: $"{PermissionsModel.Permission.View}")]
        public async Task<ActionResult> Index()
        {
            var entities = _entitybusiness.GetEntities();
            
            var create = await _authorizationService.AuthorizeAsync(User, PermissionsModel.Entity.Create);
            var edit = await _authorizationService.AuthorizeAsync(User, PermissionsModel.Entity.Edit);
            var delete = await _authorizationService.AuthorizeAsync(User, PermissionsModel.Entity.Delete);

            var response = new ManagePermission()
            {
                CreatePermission = new CreatePermission(),
                CreateEntity = new CreateEntity(),
                Create = create.Succeeded,
                Edit = edit.Succeeded,
                Delete = delete.Succeeded
            };

            response.CreatePermission.Entities = entities.Select(entity =>
                new SelectListItem(entity.Name, entity.EntityId.ToString(), false)).ToList();

            return View(response);
        }

        [Authorize(policy: $"{PermissionsModel.Permission.Edit}")]
        public async Task<IActionResult> Update([FromBody] PermissionViewModel model)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(model.RoleId);
                var claims = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claims)
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                }
                var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
                foreach (var claim in selectedClaims)
                {
                    await _roleManager.AddPermissionClaim(role, claim.Value);
                }

                TempData["SuccessMessage"] = "Permission updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Employee creation failed.";
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        public async Task<IActionResult> CheckPermissions(string policyname)
        {
            try
            {
                var hasPermission = await _authorizationService.AuthorizeAsync(User, policyname);
                return Ok(new { hasPermission = hasPermission.Succeeded });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetPermissionByRole([FromBody] PaginationFilter filter)
        {
            var response = new PermissionViewModel();
            var model = new List<RoleClaimsViewModel>();

            var data = await _business.GetPermissionsWithFilters(filter);
            var allPermissions = data.Data;

            var role = await _roleManager.FindByIdAsync(filter.roleId);
            var claims = await _roleManager.GetClaimsAsync(role);
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            var roleClaimValues = claims.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();

            foreach (var permission in allPermissions)
            {
                var claim = new RoleClaimsViewModel();
                claim.Type = permission.Entity.Name;
                claim.Value = permission.Value;

                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    claim.Selected = true;
                }

                model.Add(claim);
            }
            response.RoleClaims = model;

            var dataSrc = new
            {
                filter.draw,
                recordsTotal = data.TotalCount,
                recordsFiltered = data.TotalCount,
                data = response.RoleClaims
            };
            return Json(dataSrc);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPermission([FromBody] PaginationFilter filter)
        {
            var data = await _business.GetEntitiesWithFilters(filter);

            var mappedData = data.Data.Select(entity =>
            {
                var viewPermissionModel = _mapper.Map<ViewEntityModel>(entity);
                viewPermissionModel.Permissions = _mapper.Map<List<ViewPermissionModel>>(entity.Permissions);
                return viewPermissionModel;
            });

            var dataSrc = new
            {
                filter.draw,
                recordsTotal = data.TotalCount,
                recordsFiltered = data.TotalCount,
                data = mappedData
            };
            return Json(dataSrc);
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.Permission.Create}")]
        public async Task<IActionResult> Create(ManagePermission request)
        {
            try
            {
                await _business.Create(request.CreatePermission);

                TempData["SuccessMessage"] = "Permission created successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Permission creation failed.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.Permission.Edit}")]
        public async Task<IActionResult> Edit(ManagePermission request)
        {
            try
            {
                await _business.Update(request.CreatePermission);

                TempData["SuccessMessage"] = "Record updated successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Record update failed.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.Permission.Delete}")]
        public async Task<IActionResult> Delete(string Id)
        {
            try
            {
                await _business.Delete(Id);
                TempData["SuccessMessage"] = "Record deleted successfully.";

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Record delete failed.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.Entity.Delete}")]
        public async Task<IActionResult> DeletePermission(string Id)
        {
            try
            {
                await _business.Delete(Id);
                TempData["SuccessMessage"] = "Record deleted successfully.";

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
