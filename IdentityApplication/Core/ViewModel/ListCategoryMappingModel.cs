namespace IdentityApplication.Core.ViewModel
{
    public class ListCategoryMappingModel
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string SelectedCategory { get; set; }
        public Guid SubCategoryId { get; set; }
        public string SelectedSubCategory { get; set; }
    }
}
