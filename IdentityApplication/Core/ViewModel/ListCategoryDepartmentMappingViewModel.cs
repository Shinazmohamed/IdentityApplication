namespace IdentityApplication.Core.ViewModel
{
    public class ListCategoryDepartmentMappingViewModel
    {
        public Guid? DepartmentId { get; set; }
        public string SelectedDepartment { get; set; }
        public Guid CategoryId { get; set; }
        public string SelectedCategory { get; set; }
    }
}
