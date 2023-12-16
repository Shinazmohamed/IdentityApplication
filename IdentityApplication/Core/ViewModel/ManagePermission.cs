using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Core.ViewModel
{
    public class ManagePermission
    {
        public Guid EntityId { get; set; }
        public string Entity { get; set; }
        public Guid PermissionId { get; set; }
        public string Value { get; set; }
        public string SelectedEntity { get; set; }
        public List<SelectListItem> Entities { get; set; }

        public BasePermissionViewModel EntityPermission { get; set; }
        public BasePermissionViewModel PermissionPermission { get; set; }
    }
}
