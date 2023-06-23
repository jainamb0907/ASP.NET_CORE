using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Practical_17_Core.Models;

namespace Practical_17_Core.Controllers
{
   
        public class ClaimsController : Controller
        {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public ClaimsController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
            {
                this._userManager = userManager;
            
            this._signInManager = signInManager;
                _roleManager = roleManager;
            }

            [Authorize(Policy = "AdminRolePolicy")]
            public ActionResult Index()
            {
                var userList = _userManager.Users.ToList();
                return View(userList);
            }

            [Authorize(Policy = "AdminRolePolicy")]
            public async Task<ActionResult> Details(string id)
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user is not null)
                {
                    // list of user roles
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var User = new User { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, UserName = user.UserName, Email = user.Email, Claims = userRoles };
                    return View(User);
                }
                return View("_NotFound", new ErrorViewModel { ErrorMessage = "No user found with this Id" });
            }

            [HttpGet]
            [Authorize(Policy = "AdminRolePolicy")]
            public async Task<IActionResult> Claims(string id)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is not null)
                {
                    var existingUserRoles = await _userManager.GetRolesAsync(user);
                    var model = new UserClaimsViewModel { UserId = id };

                    foreach (var role in _roleManager.Roles)
                    {
                        UserClaim userClaim = new UserClaim { ClaimType = role.Name };

                        if (existingUserRoles.Any(r => r.ToLower() == role.Name.ToLower()))
                        {
                            userClaim.IsSelected = true;
                        }
                        model.Claims?.Add(userClaim);
                    }
                    return View(model);
                }
                return View("_NotFound", new ErrorViewModel { ErrorMessage = "No claims found with this Id" });
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            [Authorize(Policy = "AdminRolePolicy")]
            public async Task<IActionResult> Claims(UserClaimsViewModel viewModel)
            {
                var user = await _userManager.FindByIdAsync(viewModel.UserId);
                if (user is not null)
                {
                    var claims = await _userManager.GetRolesAsync(user);
                    var result = await _userManager.RemoveFromRolesAsync(user, claims);

                    if (!result.Succeeded)
                    {
                        // not able to delete claims
                        return RedirectToAction("Details");
                    }
                    result = await _userManager.AddToRolesAsync(user, viewModel.Claims?.Where(c => c.IsSelected).Select(c => c.ClaimType?.ToUpper()));
                    if (!result.Succeeded)
                    {
                        // not able to add claims
                        return RedirectToAction("Details");
                    }
                    return RedirectToAction("Details", new { id = viewModel.UserId });
                }
                return View();
            }
        }
    }

