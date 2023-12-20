using IdentityApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IdentityApplication.Middleware
{
    public class CustomSignInManager : SignInManager<ApplicationUser>
    {
        public CustomSignInManager(UserManager<ApplicationUser> userManager,
                                   IHttpContextAccessor contextAccessor,
                                   IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
                                   IOptions<IdentityOptions> optionsAccessor,
                                   ILogger<SignInManager<ApplicationUser>> logger,
                                   IAuthenticationSchemeProvider schemes,
                                   IUserConfirmation<ApplicationUser> confirmation)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }

        public override async Task<SignInResult> CheckPasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure)
        {
            // implment this later.
            //if (user != null && user.IsLoggedIn == true)
            //{
            //    return SignInResult.LockedOut;
            //}

            var result = await base.CheckPasswordSignInAsync(user, password, lockoutOnFailure);

            if (result.Succeeded)
            {
                // Update the IsLoggedIn property
                user.IsLoggedIn = true;
                await UserManager.UpdateAsync(user);
            }

            return result;
        }
    }

}
