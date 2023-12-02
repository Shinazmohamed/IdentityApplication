namespace IdentityApplication.Core.ViewModel
{
    public class MenuViewModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public IList<SubMenuViewModel>? SubMenu { get; set; }
    }

    public class SubMenuViewModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
    }
}
