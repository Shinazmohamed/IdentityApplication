using IdentityApplication.Areas.Identity.Data;

namespace IdentityApplication.Core.Entities
{
    public class SubMenuRole
    {
        public Guid SubMenuId { get; set; }

        public SubMenu SubMenu { get; set; }

        public string Id { get; set; }

        public ApplicationRole Role { get; set; }
    }
}
