namespace IdentityApplication.Core.Entities
{
    public class SubCategory
    {
        public Guid SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }

        public List<CategoryMapping> CategorySubcategories { get; set; }
    }
}
