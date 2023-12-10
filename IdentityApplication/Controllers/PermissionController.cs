using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Helpers;
using IdentityApplication.Core.Permission;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApplication.Controllers
{
    [Authorize]
    public class PermissionController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager; 
        private readonly IAuthorizationService _authorizationService;
        private readonly IPermissionBusiness _business;
        public PermissionController(RoleManager<IdentityRole> roleManager, IAuthorizationService authorizationService, IPermissionBusiness business)
        {
            _roleManager = roleManager;
            _authorizationService = authorizationService;
            _business = business;
        }
        public async Task<ActionResult> Index(string roleId)
        {
            var model = new PermissionViewModel();
            var allPermissions = new List<RoleClaimsViewModel>();
            allPermissions.GetPermissions(typeof(PermissionsModel.Employees), roleId);
            var role = await _roleManager.FindByIdAsync(roleId);
            model.RoleId = roleId;
            var claims = await _roleManager.GetClaimsAsync(role);
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            var roleClaimValues = claims.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            model.RoleClaims = allPermissions;
            return View(model);
        }
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
                var hasDeletePermission = await _authorizationService.AuthorizeAsync(User, policyname);
                return Ok(new { hasDeletePermission = hasDeletePermission.Succeeded });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] PaginationFilter filter)
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
                claim.Type = permission.Entity;
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
    }
}
