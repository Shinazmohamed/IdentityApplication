using System.ComponentModel.DataAnnotations;

namespace IdentityApplication.Core.Entities
{
    public class DepartmentCategory
    {
        [Key]
        public Guid DepartmentCategoryId { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
