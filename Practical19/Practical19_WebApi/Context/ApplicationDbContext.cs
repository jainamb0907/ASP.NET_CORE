using System.Data;
using Practical19_WebApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Practical19_WebApi.Context;
public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        Guid roleId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();

        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Name = "Admin",
            NormalizedName = "ADMIN",
            Id = roleId.ToString(),
            ConcurrencyStamp = roleId.ToString(),
        });

        User user = new User
        {
            Id = userId.ToString(),
            FirstName = "Admin",
            LastName = "Jainam",
            Email = "admin@gmail.com",
            UserName = "admin@gmail.com",
            NormalizedEmail = "ADMIN@GMAIL.COM",
            NormalizedUserName = "ADMIN@GMAIL.COM",
        };
        PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
        user.PasswordHash = passwordHasher.HashPassword(user, "Admin@2002");

        builder.Entity<User>().HasData(user);

        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = roleId.ToString(),
            UserId = userId.ToString()
        });
    }
}
