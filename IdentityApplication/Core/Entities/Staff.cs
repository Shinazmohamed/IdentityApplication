using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityApplication.Core.Entities
{
    public class Staff
    {
        public Guid StaffId { get; set; }
        public string EmployeeCode { get; set; }
        public Guid? LocationId { get; set; }
        public Location Location { get; set; }

        [ForeignKey("Team")]
        public Guid? TeamId { get; set; }
        public Team Team { get; set; }

    }
}
