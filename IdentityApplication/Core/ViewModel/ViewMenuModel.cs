namespace IdentityApplication.Core.ViewModel
{
    public class ViewMenuModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public IList<ViewSubMenuModel>? SubMenu { get; set; }
    }

    public class ViewSubMenuModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
    }
}
