﻿namespace IdentityApplication.Core.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid? DepartmentId { get; set; }
        public Department Department { get; set; }
        public List<SubCategory> SubCategories { get; set; }
    }
}
