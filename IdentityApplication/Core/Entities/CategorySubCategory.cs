﻿using System.ComponentModel.DataAnnotations;

namespace IdentityApplication.Core.Entities
{
    public class CategorySubCategory
    {
        [Key]
        public Guid CategorySubCategoryId { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}
