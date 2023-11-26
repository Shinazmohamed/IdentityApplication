using IdentityApplication.Core.Entities;

namespace IdentityApplication.Core.ViewModel
{
    public class ListCategoryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<SubCategory> SubCategories { get; set; }
    }
}
