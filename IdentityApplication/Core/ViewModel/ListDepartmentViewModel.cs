using IdentityApplication.Core.Entities;

namespace IdentityApplication.Core.ViewModel
{
    public class ListDepartmentViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<Category> Categories { get; set; }
    }
}
