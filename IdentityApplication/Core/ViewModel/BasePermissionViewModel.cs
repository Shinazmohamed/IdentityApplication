namespace IdentityApplication.Core.ViewModel
{
    public class BasePermissionViewModel
    {
        public bool Create { get; set; } = false;
        public bool Edit { get; set; } = false;
        public bool View { get; set; } = false;
        public bool Delete { get; set; } = false;
        public bool IsAdminOrSuperDev { get; set; }
    }
}
