using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace IdentityApplication.Core.ViewModel
{
    public class CreateCategorySubCategoryRequest
    {
        [Required]
        [Display(Name = "Category")]
        public string SelectedCategory { get; set; }
        public List<SelectListItem> Categories { get; set; }
        [Required]
        [Display(Name = "Sub Category")]
        public string SelectedSubCategory { get; set; }
        public List<SelectListItem> SubCategories { get; set; }
        public string SelectedSubCategoryId { get; set; }
    }
}
