using System.ComponentModel.DataAnnotations;

namespace IdentityApplication.Core.Entities
{
    public class CategoryMapping
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid SubCategoryId { get; set; }

        public Category Category { get; set; }
        public SubCategory SubCategory { get; set; }

    }
}
