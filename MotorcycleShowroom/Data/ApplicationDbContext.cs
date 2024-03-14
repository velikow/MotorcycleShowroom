using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MotorcycleShowroom.Models;

namespace MotorcycleShowroom.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MotorcycleShowroom.Models.BMW>? BMW { get; set; }
        public DbSet<Like> Likes { get; set; }  
        public DbSet<ApplicationUser> User { get; set; }

    }
}