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
        public DbSet<Image> Image { get; set; }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Image>()
                .HasOne(i => i.BMW)             // Image has one BMW
                .WithMany(b => b.Images)         // BMW has many Images
                .HasForeignKey(i => i.BMWId);   // Foreign key in Image entity

            // Additional configuration if needed
        }

    }
}