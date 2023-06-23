using System.Data;
using Microsoft.Win32;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Practical19_MVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Practical19_MVC.Controllers;
public class RolesController : Controller
{
    private readonly HttpClient _httpClient = new HttpClient();
    public RolesController(IConfiguration configuration)
    {
        _httpClient.BaseAddress = new Uri(configuration["AppSettings:BaseUrl"]);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("api/v1/users");
        if (response.IsSuccessStatusCode)
        {
            var users = await response.Content.ReadFromJsonAsync<IEnumerable<UserViewModel>>();
            return View(users);
        }
        return View("_NotFound", new ErrorViewModel { ErrorMessage = "No users found" });
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Claims(string id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/v1/roles");
        if (response.IsSuccessStatusCode)
        {
            HttpResponseMessage fetchUserRolesresponse = await _httpClient.GetAsync($"api/v1/users/{id}/roles");
            if (fetchUserRolesresponse.IsSuccessStatusCode)
            {
                var existingUserRoles = await fetchUserRolesresponse.Content.ReadFromJsonAsync<IEnumerable<string>>();
                var roles = await response.Content.ReadFromJsonAsync<IEnumerable<RoleViewModel>>();

                var userRolesString = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                var model = new UserClaimsViewModel { UserId = id };

                foreach (var role in roles)
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
            return View("_NotFound", new ErrorViewModel { ErrorMessage = "No user found" });
        }
        return View("_NotFound", new ErrorViewModel { ErrorMessage = "No user found" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Claims(UserClaimsViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var selectedRoles = viewModel.Claims.Where(c => c.IsSelected == true).Select(c => c.ClaimType).ToList();
        var payload = new { Roles = selectedRoles };
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/v1/users/{viewModel.UserId}/roles", payload);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index", "Roles");
        }
        return RedirectToAction("Index");
    }
}
