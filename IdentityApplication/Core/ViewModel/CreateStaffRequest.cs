using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Core.ViewModel
{
    public class CreateStaffRequest
    {
        public string EmployeeCode { get; set; }
        public string SelectedStaffTeam { get; set; }
        public List<SelectListItem> StaffTeamCollection { get; set; }
    }
}
