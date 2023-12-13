namespace IdentityApplication.Core.ViewModel
{
    public class SubMenuViewModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
        public bool Selected { get; set; }
    }
}
