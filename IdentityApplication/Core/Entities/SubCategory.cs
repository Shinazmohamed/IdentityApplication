namespace IdentityApplication.Core.Entities
{
    public class SubCategory
    {
        public Guid SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public Guid? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
