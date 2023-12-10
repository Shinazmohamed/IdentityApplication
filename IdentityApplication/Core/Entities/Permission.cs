using System.ComponentModel.DataAnnotations;

namespace IdentityApplication.Core.Entities
{
    public class Permission
    {
        [Key]
        public Guid Id { get; set; }
        public string Value { get; set; }
        public string Entity { get; set; }
    }
}
