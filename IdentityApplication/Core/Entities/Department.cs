﻿namespace IdentityApplication.Core.Entities
{
    public class Department
    {
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public List<Category> Categories { get; set;}
    }
}
