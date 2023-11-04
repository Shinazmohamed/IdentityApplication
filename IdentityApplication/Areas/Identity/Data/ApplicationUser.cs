using IdentityApplication.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityApplication.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public Guid LocationId { get; set; }
    public Location Location { get; set; }
}

public class ApplicationRole : IdentityRole
{

}

