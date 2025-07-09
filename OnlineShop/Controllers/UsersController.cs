using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Areas.Identity.Data;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class UsersController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<OnlineShopUser> _userManager;

        public UsersController(RoleManager<IdentityRole> roleManager, UserManager<OnlineShopUser> userManager)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RoleCreate()
        {
            return View();
        }
        public IActionResult RoleList()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(OnlineShopUserRole role)
        {
            var roleExist = await _roleManager.RoleExistsAsync(role.RoleName); 
            if (!roleExist)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(role.RoleName));
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RoleEdit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.users = await _userManager.GetUsersInRoleAsync(role.Name);
            }
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> RoleEdit(IdentityRole role)
        {
            if (role == null)
            {
                return NotFound();
            }
            else
            {
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            List<OnlineShopUserViewModel> userViewModels = new List<OnlineShopUserViewModel>();
            var AllUsers = _userManager.Users.ToList();
            foreach (var user in AllUsers)
            {
                userViewModels.Add(new OnlineShopUserViewModel
                {
                    User = user,
                    RoleName = string.Join("", await _userManager.GetRolesAsync(user))
                });
            }

            return View(userViewModels);
        }


        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id) 
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) 
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(string id) 
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded) 
            {
                return RedirectToAction("UserList");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View("UserList");
        }
    }
}
