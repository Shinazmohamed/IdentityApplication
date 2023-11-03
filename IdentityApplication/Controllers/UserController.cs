using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(IUnitOfWork unitOfWork, SignInManager<ApplicationUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var users = _unitOfWork.User.GetUsers();
            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = _unitOfWork.User.GetUser(id);
            var roles = _unitOfWork.Role.GetRoles();

            var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

            var roleItems = roles.Select(role => 
            new SelectListItem(
                role.Name, 
                role.Id, 
                userRoles.Any(ur => ur.Contains(role.Name)))).ToList();
            var vm = new EditUserViewModel { User = user, Roles = roleItems };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync(EditUserViewModel request)
        {
            var user = _unitOfWork.User.GetUser(request.User.Id);
            if(user == null)
            {
                return NotFound();
            }

            var rolesToAdd = new List<string>();
            var rolesToRemove = new List<string>();

            var userRolesInDb = await _signInManager.UserManager.GetRolesAsync(user);
            foreach(var role in request.Roles) 
            {
                var assignedInDb = userRolesInDb.FirstOrDefault(ur => ur == role.Text);
                if (role.Selected)
                {
                    if (assignedInDb == null)
                        rolesToAdd.Add(role.Text);
                    
                }
                else
                {
                    if (assignedInDb != null)
                        rolesToRemove.Add(role.Text);
                    
                }
            }

            if (rolesToAdd.Any())
            {
                await _signInManager.UserManager.AddToRolesAsync(user, rolesToAdd);
            }

            if (rolesToRemove.Any())
            {
                await _signInManager.UserManager.RemoveFromRolesAsync(user, rolesToRemove);
            }

            user.Email = request.User.Email;

            _unitOfWork.User.UpdateUser(user);

            return RedirectToAction("Edit", new {Id = request.User.Id});
        }
    }
}
