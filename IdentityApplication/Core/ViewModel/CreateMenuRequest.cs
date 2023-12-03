using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityApplication.Core.ViewModel
{
    public class CreateMenuRequest
    {
        [Display(Name = "Menu")]
        public string? SelectedMenu { get; set; }
        public List<SelectListItem> Menus { get; set; }
        [DisplayName("Display Name")]
        public string? DisplayName { get; set; }

        [DisplayName("Controller")]
        public string? Controller { get; set; }

        [DisplayName("Method")]
        public string? Method { get; set; }
        [Required]
        public bool IsParent { get; set; }
    }
}
