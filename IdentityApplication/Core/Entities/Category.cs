namespace IdentityApplication.Core.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }

        public List<CategoryMapping> CategorySubcategories { get; set; }
    }
}
