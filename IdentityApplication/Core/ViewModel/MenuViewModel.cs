namespace IdentityApplication.Core.ViewModel
{
    public class MenuViewModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public int Sort { get; set; }
        public IList<SubMenuViewModel>? SubMenu { get; set; }
    }
}
