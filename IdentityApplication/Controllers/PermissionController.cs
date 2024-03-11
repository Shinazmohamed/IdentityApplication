using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Entities;
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
    public class PermissionController : BaseController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IPermissionBusiness _business;
        private readonly IEntityBusiness _entitybusiness;
        private readonly IMapper _mapper;
        private readonly ILogger<PermissionController> _logger;
        private readonly INotyfService _notyf;
        public PermissionController(RoleManager<IdentityRole> roleManager, IAuthorizationService authorizationService, IPermissionBusiness business, IMapper mapper, IEntityBusiness entitybusiness, ILogger<PermissionController> logger, INotyfService notyf)
        {
            _roleManager = roleManager;
            _authorizationService = authorizationService;
            _business = business;
            _mapper = mapper;
            _entitybusiness = entitybusiness;
            _logger = logger;
            _notyf = notyf;
        }

        [Authorize(policy: $"{PermissionsModel.PermissionPermission.View}")]
        public async Task<ActionResult> Index()
        {
            var response = new ManagePermission();
            try
            {
                var entities = _entitybusiness.GetEntities();

                var entity_create = await _authorizationService.AuthorizeAsync(User, PermissionsModel.EntityPermission.Create);
                var entity_edit = await _authorizationService.AuthorizeAsync(User, PermissionsModel.EntityPermission.Edit);
                var entity_delete = await _authorizationService.AuthorizeAsync(User, PermissionsModel.EntityPermission.Delete);
                var entityPermission = new BasePermissionViewModel()
                {
                    Create = entity_create.Succeeded,
                    Edit = entity_edit.Succeeded,
                    Delete = entity_delete.Succeeded
                };

                var permission_create = await _authorizationService.AuthorizeAsync(User, PermissionsModel.PermissionPermission.Create);
                var permission_edit = await _authorizationService.AuthorizeAsync(User, PermissionsModel.PermissionPermission.Edit);
                var permission_delete = await _authorizationService.AuthorizeAsync(User, PermissionsModel.PermissionPermission.Delete);
                var permissionPermission = new BasePermissionViewModel()
                {
                    Create = permission_create.Succeeded,
                    Edit = permission_edit.Succeeded,
                    Delete = permission_delete.Succeeded
                };

                response.EntityPermission = entityPermission;
                response.PermissionPermission = permissionPermission;

                if (entities.Any())
                {
                    response.Entities = entities.Select(entity =>
                        new SelectListItem(entity.Name, entity.EntityId.ToString(), false)).ToList();
                }
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(PermissionController));
            }

            return View(response);
        }

        [Authorize(policy: $"{PermissionsModel.PermissionPermission.Edit}")]
        public async Task<IActionResult> Update([FromBody] PermissionViewModel model)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(model.RoleId);
                var claims = await _roleManager.GetClaimsAsync(role);
                if(claims.Any())
                {
                    foreach (var claim in claims)
                    {
                        await _roleManager.RemoveClaimAsync(role, claim);
                    }
                }
                if (model.RoleClaims.Any())
                {
                    var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
                    foreach (var claim in selectedClaims)
                    {
                        await _roleManager.AddPermissionClaim(role, claim.Value);
                    }
                }
                _notyf.Success("Permission updated successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(PermissionController));
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
                _notyf.Error("Operation Failed. Please contact administrator");
                _logger.LogError(ex, "{Controller} All function error", typeof(PermissionController));
            }
            return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> GetPermissionByRole([FromBody] PaginationFilter filter)
        {
            var response = new PermissionViewModel();
            var paginationResponse = new PaginationResponse<Permission>();
            try
            {
                var model = new List<RoleClaimsViewModel>();
                var permission = await _business.GetPermissionsWithFilters(filter);
                if(permission != null)
                {
                    if (permission.Data.Any())
                    {
                        var role = await _roleManager.FindByIdAsync(filter.roleId);
                        var claims = await _roleManager.GetClaimsAsync(role);

                        var allClaimValues = permission.Data.Select(a => a.Value).ToList();
                        var roleClaimValues = claims.Select(a => a.Value).ToList();
                        var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();

                        foreach (var item in permission.Data)
                        {
                            var claim = new RoleClaimsViewModel();
                            claim.Type = item.Entity.Name;
                            claim.Value = item.Value;

                            if (authorizedClaims.Any(a => a == item.Value))
                            {
                                claim.Selected = true;
                            }

                            model.Add(claim);
                        }
                        response.RoleClaims = model;
                    }
                }
                paginationResponse.Data = permission.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Controller} All function error", typeof(PermissionController));
            }
            return Json(new
            {
                filter.draw,
                recordsTotal = paginationResponse.TotalCount,
                recordsFiltered = paginationResponse.TotalCount,
                data = response.RoleClaims
            });
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
        [Authorize(policy: $"{PermissionsModel.PermissionPermission.Create}")]
        public async Task<IActionResult> Create(ManagePermission request)
        {
            try
            {
                await _business.Create(request);
                _notyf.Success("Permission created successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Error Occured.");
                _logger.LogError(ex, "{Controller} All function error", typeof(PermissionController));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.PermissionPermission.Edit}")]
        public async Task<IActionResult> Edit(ManagePermission request)
        {
            try
            {
                await _business.Update(request);
                _notyf.Success("Permission updated successfully.");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Record update failed.";
                _logger.LogError(ex, "{Controller} All function error", typeof(PermissionController));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.PermissionPermission.Delete}")]
        public async Task<IActionResult> Delete(string Id)
        {
            try
            {
                await _business.Delete(Id);
                _notyf.Success("Permission deleted successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Error Occured.");
                _logger.LogError(ex, "{Controller} All function error", typeof(PermissionController));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.EntityPermission.Delete}")]
        public async Task<IActionResult> DeletePermission(string Id)
        {
            try
            {
                await _business.Delete(Id);
                _notyf.Success("Entity deleted successfully.");
            }
            catch (Exception ex)
            {
                _notyf.Error("Error Occured.");
                _logger.LogError(ex, "{Controller} All function error", typeof(PermissionController));
            }
            return RedirectToAction("Index");
        }
    }
}
