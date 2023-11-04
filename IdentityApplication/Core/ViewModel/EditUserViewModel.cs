using IdentityApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Core.ViewModel
{
    public class EditUserViewModel
    {
        public ApplicationUser User { get; set; }
        public IList<SelectListItem> Roles { get; set; }
        public IList<SelectListItem> Locations { get; set; }
    }
}
