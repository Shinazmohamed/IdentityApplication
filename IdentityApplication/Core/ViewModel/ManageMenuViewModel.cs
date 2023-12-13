namespace IdentityApplication.Core.ViewModel
{
    public class ManageMenuViewModel
    {
        public string RoleId { get; set; }
        public IList<MenuData>? menuData { get; set; }
    }

    public class MenuData
    {
        public Guid Id { get; set; }
        public bool Selected { get; set; }
    }
}
