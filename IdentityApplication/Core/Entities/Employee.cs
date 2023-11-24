using System.ComponentModel.DataAnnotations;

namespace IdentityApplication.Core.Entities
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }
        public string? LocationName { get; set; }
        public string? DepartmentName { get; set; }
        public string? CategoryName { get; set; }
        public string? SubCategoryName { get; set; }
        public string? E1 { get; set; }
        public string? E2 { get; set; }
        public double? C { get; set; }
        public string? M1 { get; set; }
        public string? M2 { get; set; }
    }
}
