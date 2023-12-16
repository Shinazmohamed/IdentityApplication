using Microsoft.AspNetCore.Identity;

namespace IdentityApplication.Core.ViewModel
{
    public class RolesViewModel : BasePermissionViewModel
    {
        public IEnumerable<IdentityRole> Roles { get; set; }
        public bool AssignPermission { get; set; }
        public bool AssignMenu { get; set; }
    }
}
