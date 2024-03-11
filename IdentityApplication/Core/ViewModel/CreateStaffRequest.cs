using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Core.ViewModel
{
    public class CreateStaffRequest
    {
        public string EmployeeCode { get; set; }
        public string SelectedTeam { get; set; }
        public List<SelectListItem> TeamCollection { get; set; }
    }
}
