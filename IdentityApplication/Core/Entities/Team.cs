namespace IdentityApplication.Core.Entities
{
    public class Team
    {
        public Guid TeamId { get; set; }
        public string TeamName { get; set; }
        public List<Staff> Staffs { get; set; }
    }
}
