namespace IdentityApplication.Core.Entities
{
    public class Staff
    {
        public Guid StaffId { get; set; }
        public string EmployeeCode { get; set; }
        public Guid? LocationId { get; set; }
        public Location Location { get; set; }
    }
}
