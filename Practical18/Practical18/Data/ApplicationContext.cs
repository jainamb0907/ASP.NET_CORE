using Microsoft.EntityFrameworkCore;
using Practical18.Model;
using System;

namespace Practical18.Data
{
    public class ApplicationContext :DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
    }
}
