using IdentityApplication.Core.Entities;

namespace IdentityApplication.Core.ViewModel
{
    public class ListUsersModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string LocationName { get; set; }
        public string Role { get; set; }
    }
}
