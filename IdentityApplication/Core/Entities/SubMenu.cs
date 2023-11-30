using System.ComponentModel.DataAnnotations;

namespace IdentityApplication.Core.Entities
{
    public class SubMenu
    {
        [Key]
        public Guid SubMenuId { get; set; }
        public string DisplayName { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
        public Guid? MenuId { get; set; }
        public Menu? Menu { get; set; }
    }
}
