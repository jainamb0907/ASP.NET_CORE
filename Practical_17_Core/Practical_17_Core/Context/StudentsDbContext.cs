using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Practical_17_Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Practical_17_Core.Context
{

    public class StudentsDbContext : IdentityDbContext<User>
    {
        public StudentsDbContext(DbContextOptions<StudentsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var forignkey in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                forignkey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            builder.Entity<User>(entity => entity.ToTable("Users"));
            builder.Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"));
            builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("UserRoles"));
            builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("UserClaims"));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("UserLogins"));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("RoleClaims"));
            builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("UserTokens"));

            // Seeding two roles by default
            var adminRole = new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Admin" };
            var normalRole = new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Normal" };

            builder.Entity<IdentityRole>()
                .HasData(adminRole, normalRole);
        }
        public DbSet<Student> Students { get; set; } = null!;
    }
}
