using System.ComponentModel.DataAnnotations;

namespace IdentityApplication.Core.Entities
{
    public class Entity
    {
        [Key]
        public Guid EntityId { get; set; }
        public string Name { get; set; }

        public List<Permission> Permissions { get; set; }
    }
}
