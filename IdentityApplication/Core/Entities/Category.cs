namespace IdentityApplication.Core.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<DepartmentCategory> DepartmentCategories { get; set; }
        public List<CategorySubCategory> CategorySubCategories { get; set; }
    }
}
