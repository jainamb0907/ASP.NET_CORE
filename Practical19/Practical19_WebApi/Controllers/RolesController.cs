using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practical19_WebApi.Entities;
using Practical19_WebApi.Models;

namespace Practical19_WebApi.Controllers;

[ApiController]
[Route("api/v1/roles")]
public class RolesController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolesController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return Ok(roles);
    }

    [HttpGet("{id}", Name = "GetRoleByIdAsync")]
    public async Task<IActionResult> GetRoleByIdAsync(string id)
    {
        var role = await _roleManager.Roles.FirstOrDefaultAsync(role => role.Id == id);
        if(role is null)
        {
            return NotFound();
        }
        return Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRolesAsync(CreateEditRoleDto roleDto)
    {
        IdentityRole identityRole = new IdentityRole { Name = roleDto.RoleName, NormalizedName = roleDto.RoleName?.ToUpper() };
        await _roleManager.CreateAsync(identityRole);
        return CreatedAtRoute("GetRoleByIdAsync", new { id = identityRole.Id }, identityRole);
    }
}
