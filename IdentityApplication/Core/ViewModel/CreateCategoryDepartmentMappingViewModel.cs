using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace IdentityApplication.Core.ViewModel
{
    public class CreateCategoryDepartmentMappingViewModel
    {
        [Required]
        [Display(Name = "Department")]
        public string SelectedDepartment { get; set; }
        public List<SelectListItem> Departments { get; set; }
        [Required]
        [Display(Name = "Category")]
        public string SelectedCategory { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public string SelectedCategoryId { get; set; }
    }
}
