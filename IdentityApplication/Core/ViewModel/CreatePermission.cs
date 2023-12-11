using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace IdentityApplication.Core.ViewModel
{
    public class CreatePermission
    {
        public string Value { get; set; }
        public string Entity { get; set; }

        [Display(Name = "Entity")]
        public string SelectedEntity { get; set; }
        public List<SelectListItem> Entities { get; set; }
    }
}
