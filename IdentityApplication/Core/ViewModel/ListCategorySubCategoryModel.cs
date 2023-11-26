namespace IdentityApplication.Core.ViewModel
{
    public class ListCategorySubCategoryModel
    {
        public Guid? CategoryId { get; set; }
        public string SelectedCategory { get; set; }
        public Guid SubCategoryId { get; set; }
        public string SelectedSubCategory { get; set; }
    }
}
