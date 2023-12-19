﻿using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.PermissionHelper;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<UserController> _logger;
        private readonly IUserBusiness _business;
        private readonly IConfiguration _configuration;
        public UserController(IUnitOfWork unitOfWork, SignInManager<ApplicationUser> signInManager, ILogger<UserController> logger, IUserBusiness business, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _logger = logger;
            _business = business;
            _configuration = configuration;
        }

        [Authorize(policy: $"{PermissionsModel.UserPermission.View}")]
        public IActionResult Index()
        {
            var users = _unitOfWork.User.GetUsersWithRoles();
            return View(users);

        }

        [Authorize(policy: $"{PermissionsModel.UserPermission.Edit}")]
        public async Task<IActionResult> Edit(string userId)
        {
            var response = new EditUserViewModel();
            try
            {
                if(string.IsNullOrEmpty(userId)) return View(response);

                var user = _unitOfWork.User.GetUser(userId);
                var roles = _unitOfWork.Role.GetRoles();
                var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

                var roleItems = roles.Select(role =>
                new SelectListItem(
                    role.Name,
                    role.Id,
                    userRoles.Any(ur => ur.Contains(role.Name)))).ToList();

                var locations = _unitOfWork.Location.GetLocations();
                var locationItems = locations.Select(location =>
                new SelectListItem(
                    location.LocationName,
                    location.LocationId.ToString(),
                    locations.Any(e => e.LocationId == user.LocationId))).ToList();

                response.User = user;
                response.Roles = roleItems;
                response.Locations = locationItems;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error Occured! Please contact admin";
                _logger.LogError(ex, "{Controller} All function error", typeof(UserController));
            }

            return View(response);
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.UserPermission.Edit}")]
        public async Task<IActionResult> OnPostAsync(EditUserViewModel request)
        {
            try
            {
                await _business.UpdateUser(request);
                TempData["SuccessMessage"] = "User is updated successfull.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "User is updated unsuccessfull";
                _logger.LogError(ex, "{Controller} All function error", typeof(UserController));
            }


            return RedirectToAction("Edit", new { userId = request.User.Id });
        }

        [HttpPost]
        [Authorize(policy: $"{PermissionsModel.UserPermission.ResetPassword}")]
        public async Task<IActionResult> ResetPassword(EditUserViewModel request)
        {
            var defaultPassword = _configuration.GetSection("AppSettings")["DefaultPassword"];
            if (string.IsNullOrEmpty(defaultPassword))
            {
                TempData["ErrorMessage"] = "Operation failed. Please set the default password in app settings.";
                return RedirectToAction("Edit", new { userId = request.User.Id });
            }

            var result = await _business.ResetPassword(request, defaultPassword);
            if (!result)
            {
                TempData["ErrorMessage"] = "Password reset is unsuccessfull";
            }
            else
            {
                TempData["SuccessMessage"] = $"Operation Successful! Default Password set to {defaultPassword}.";
            }

            return RedirectToAction("Edit", new { userId = request.User.Id });
        }

        [Authorize(policy: $"{PermissionsModel.UserPermission.Profile}")]
        public IActionResult Profile()
        {
            return Redirect("http://localhost:5258/Identity/Account/Manage");
        }

        [Authorize(policy: $"{PermissionsModel.UserPermission.Register}")]
        public IActionResult Register()
        {        
            return Redirect("http://localhost:5258/Identity/Account/Register");
        }

    }
}
