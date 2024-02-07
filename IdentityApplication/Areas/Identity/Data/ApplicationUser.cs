using IdentityApplication.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityApplication.Areas.Identity.Data;

public class ApplicationUser : IdentityUser
{
    public Guid? LocationId { get; set; }
    public Location Location { get; set; }
}

public class ApplicationRole : IdentityRole
{
    public List<SubMenuRole> SubMenuRoles { get; set; }
}

