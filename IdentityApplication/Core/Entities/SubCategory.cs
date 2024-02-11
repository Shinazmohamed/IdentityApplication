namespace IdentityApplication.Core.Entities
{
    public class SubCategory
    {
        public Guid SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public List<CategorySubCategory> CategorySubCategories { get; set; }
    }
}
