using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Practical_17_Core.Context;
using Practical_17_Core.Models;
using Practical_17_Core.Repository;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// AutoMapper service
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// IStudentRepository service
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// Database connection and EF Core service
builder.Services.AddDbContextPool<StudentsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentsDbContext"));
});

// Identity service
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<StudentsDbContext>();

// Claims policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RegularRolePolicy", policy => policy.RequireRole("Normal"));
    options.AddPolicy("BothRolePolicy", policy =>
    {
        policy.RequireAssertion(context => context.User.IsInRole("Admin") || context.User.IsInRole("Normal"));
    });
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Users/Login";
    options.LogoutPath = "/Users/Logout";
    options.AccessDeniedPath = "/Users/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
