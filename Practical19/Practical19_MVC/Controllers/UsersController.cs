using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Practical19_MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Practical19_MVC.Controllers;
public class UsersController : Controller
{
    private readonly HttpClient _httpClient = new HttpClient();
    public UsersController(IConfiguration configuration)
    {
        _httpClient.BaseAddress = new Uri(configuration["AppSettings:BaseUrl"]);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel register)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync<RegisterViewModel>("api/v1/users/register", register);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Login");
        }
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel login, string? returnUrl)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync<LoginViewModel>("api/v1/users/login", login);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            ModelState.AddModelError(nameof(login.Password), "Invalid username or password");
            return View();
        }
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var UserData = new { Token = "", FirstName = "", LastName = "", Email = "", Id = "", Roles = new List<string>()};

            var result = JsonConvert.DeserializeAnonymousType(content, UserData);

            List<Claim> roleClaims = new List<Claim>();
            foreach (var role in result.Roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var claims = new List<Claim>{
                new Claim("Token", result.Token),
                new Claim("FirstName", result.FirstName),
                new Claim("LastName", result.LastName),
                new Claim("Id", result.Id),
                new Claim("Email", result.Email),
                new Claim(ClaimTypes.Name, result.FirstName +" "+ result.LastName),
                new Claim(ClaimTypes.Role, string.Join(", ",result.Roles)),       
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaims(roleClaims);

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                IsPersistent = login.RememberMe
            });

            return LocalRedirect(returnUrl ?? "/Home/Index");
        }
        return View();
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult AccessDenied()
    {

        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);   
        return LocalRedirect("/");
    }

    [HttpGet]
    [Authorize]
    public IActionResult Account()
    {
        if (User != null && User.Identity != null && User.Identity.IsAuthenticated && User.Claims != null)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            if (userId is not null) 
            {
                var userViewModel = new UserViewModel
                {
                    Id = userId,
                    FirstName = User.Claims.FirstOrDefault(c => c.Type == "FirstName")?.Value,
                    LastName = User.Claims.FirstOrDefault(c => c.Type == "LastName")?.Value,
                    Email = User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value,
                    Roles = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
                };
                return View(userViewModel);
            }
        }
        return View();
    }
}
