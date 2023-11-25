using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace IdentityApplication.Core.ViewModel
{
    public class CreateSubCategoryRequest
    {
        public string? Id { get; set; }
        public Guid CategoryId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string SelectedCategory { get; set; }

        public List<SelectListItem> Categories { get; set; }

        public Guid SubCategoryId { get; set; }

        [Required]
        [Display(Name = "Sub Category")]
        public string SelectedSubCategory { get; set; }

        public List<SelectListItem> SubCategories { get; set; }
    }
}
