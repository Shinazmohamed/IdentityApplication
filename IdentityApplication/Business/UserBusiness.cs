using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace IdentityApplication.Business
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserBusiness> _logger;
        public UserBusiness(SignInManager<ApplicationUser> signInManager, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task UpdateUserRole(ApplicationUser user, string selectedRole)
        {
            try
            {
                var rolesToAdd = new List<string>();
                var rolesToRemove = new List<string>();

                var userRolesInDb = await _signInManager.UserManager.GetRolesAsync(user);

                var roles = await _unitOfWork.Role.GetAll();

                foreach (var role in roles)
                {
                    var assignedInDb = userRolesInDb.FirstOrDefault(ur => ur == role.Name);
                    if (role.Id == selectedRole)
                    {
                        if (assignedInDb == null)
                            rolesToAdd.Add(role.Name);

                    }
                    else
                    {
                        if (assignedInDb != null)
                            rolesToRemove.Add(role.Name);
                    }
                }

                if (rolesToAdd.Any())
                {
                    await _signInManager.UserManager.AddToRolesAsync(user, rolesToAdd);
                }

                if (rolesToRemove.Any())
                {
                    await _signInManager.UserManager.RemoveFromRolesAsync(user, rolesToRemove);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(SubMenuBusiness));
            }
        }

        public async Task UpdateUser(EditUserViewModel request)
        {
            try
            {
                var user = _unitOfWork.User.GetUser(request.User.Id);
                if (user == null)
                {
                    return;
                }

                user.Email = request.User.Email;
                user.UserName = request.User.Email;
                user.NormalizedEmail = request.User.Email.ToUpper();
                user.NormalizedUserName = request.User.Email.ToUpper();
                user.LocationId = request.User.LocationId;
                user.LockoutEnabled = request.User.LockoutEnabled;
                if (user.LockoutEnabled)
                    user.LockoutEnd = DateTime.Now.AddMinutes(20);
                else
                    user.LockoutEnd = null;

                await UpdateUserRole(user, request.SelectedRole);

                _unitOfWork.User.UpdateUser(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(SubMenuBusiness));
            }
        }

        public async Task<bool> ResetPassword(EditUserViewModel request, string defaultpassword)
        {
            var response = false;
            try
            {
                var user = _unitOfWork.User.GetUser(request.User.Id);
                if (user == null)
                {
                    return false;
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, defaultpassword);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Business} All function error", typeof(SubMenuBusiness));
            }
            return response;
        }
    }
}
