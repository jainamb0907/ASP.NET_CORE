using AutoMapper;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Practical19_WebApi.Models;
using Practical19_WebApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Practical19_WebApi.Controllers;
[ApiController]
[Route("api/v1/users")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto register)
    {
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        var user = _mapper.Map<User>(register);
        user.UserName = register.Email;

        var result = await _userManager.CreateAsync(user, register.Password);
        if (result.Succeeded)
        {
            return Ok("Registration successful.");
        }
        return UnprocessableEntity(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto login)
    {
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
        if (result.Succeeded)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:Key"]));
            var signInCreds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
            var claimsForToken = new List<Claim>
            {
                new Claim("email", login.Email ?? "")
            };

            var jwtToken = new JwtSecurityToken(_configuration["Authentication:Issuer"], _configuration["Authentication:Audience"], claimsForToken, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), signInCreds);
            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            // getting user roles
            var user = await _userManager.FindByEmailAsync(login.Email);
            var roles = await _userManager.GetRolesAsync(user);
            var userToSend = new {Token = tokenToReturn, Id =user.Id, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, Roles = roles};
            return Ok(userToSend);
        }
        return Unauthorized();
    }

    [HttpGet("{id}/roles")]
    public async Task<IActionResult> GetRolesAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }
        var roles = await _userManager.GetRolesAsync(user);
        return Ok(roles);
    }

    [HttpPost("{id}/roles")]
    public async Task<IActionResult> AddRolesAsync(string id, AddRolesDto addRoles)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }
        var exisitngRoles = await _userManager.GetRolesAsync(user);
        try
        {
            await _userManager.RemoveFromRolesAsync(user, exisitngRoles);
            var addUserRoles = await _userManager.AddToRolesAsync(user, addRoles.Roles);
            if (addUserRoles.Succeeded)
            {
                return Ok();
            }
            return UnprocessableEntity(addUserRoles.Errors);
        }
        catch (Exception ex)
        {
            return UnprocessableEntity(ex.Message);
        }

    }

    [HttpGet]
    public async Task<IActionResult> GetUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return Ok(users);
    }
}
