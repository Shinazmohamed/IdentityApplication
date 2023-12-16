using IdentityApplication.Core.PermissionHelper;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityApplication.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthorizationService _authorizationService;
        public RolesController(RoleManager<IdentityRole> roleManager, IAuthorizationService authorizationService)
        {
            _roleManager = roleManager;
            _authorizationService = authorizationService;
        }

        [Authorize(policy: $"{PermissionsModel.RolePermission.View}")]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var create = await _authorizationService.AuthorizeAsync(User, PermissionsModel.RolePermission.Create);
            var assignPermission = await _authorizationService.AuthorizeAsync(User, PermissionsModel.RolePermission.AssignPermission);
            var assignMenu = await _authorizationService.AuthorizeAsync(User, PermissionsModel.RolePermission.AssignMenu);

            var response = new RolesViewModel()
            {
                Roles = roles,
                Create = create.Succeeded,
                AssignMenu = assignMenu.Succeeded,
                AssignPermission = assignPermission.Succeeded
            };
            return View(response);
        }
        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.RolePermission.Create}")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            return RedirectToAction("Index");
        }
    }
}
