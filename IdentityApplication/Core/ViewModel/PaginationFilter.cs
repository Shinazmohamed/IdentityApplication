namespace IdentityApplication.Core.ViewModel
{
    public class PaginationFilter
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public string location { get; set; }
        public string department { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }
    }
}
