namespace IdentityApplication.Core.ViewModel
{
    public class ViewEntityModel
    {
        public Guid EntityId { get; set; }
        public string Name { get; set; }

        public List<ViewPermissionModel> Permissions { get; set; }
    }

    public class ViewPermissionModel
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public Guid EntityId { get; set; }
    }
    
}
