using IdentityApplication.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace IdentityApplication.Core.ViewModel
{
    public class InsertEmployeeRequest
    {
        [Display(Name = "Id")]
        public string? EmployeeId { get; set; }

        [Display(Name = "E1")]
        public string? E1 { get; set; }

        [Display(Name = "E2")]
        public string? E2 { get; set; }

        [Display(Name = "M1")]
        public string? M1 { get; set; }

        [Display(Name = "M2")]
        public string? M2 { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string SelectedLocation { get; set; }

        public List<SelectListItem> Locations { get; set; } = new List<SelectListItem>();

        [Required]
        [Display(Name = "Department")]
        public string SelectedDepartment { get; set; }

        public List<SelectListItem> Departments { get; set; } = new List<SelectListItem>();

        [Required]
        [Display(Name = "Category")]
        public string SelectedCategory { get; set; }

        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

        [Required]
        [Display(Name = "Sub Category")]
        public string SelectedSubCategory { get; set; }

        public List<SelectListItem> SubCategories { get; set; } = new List<SelectListItem>();
    }

}
