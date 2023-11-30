using System.ComponentModel.DataAnnotations;

namespace IdentityApplication.Core.Entities
{
    public class Menu
    {
        [Key]
        public Guid MenuId { get; set; }
        public string DisplayName { get; set; }

        public List<SubMenu> SubMenus { get; set; }
    }
}
