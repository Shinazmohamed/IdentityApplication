using Microsoft.AspNetCore.Identity;

namespace IdentityApplication.Core.ViewModel
{
    public class RolesViewModel : BasePermissionViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }
        public bool AssignPermission { get; set; }
        public bool AssignMenu { get; set; }
    }
}
