using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MotorcycleShowroom.Models;

namespace MotorcycleShowroom.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        internal static IEnumerable<object> Images;

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
                .HasOne(image => image.BMW)          // Image has one BMW
                .WithMany(bmw => bmw.Images)         // BMW has many Images
                .HasForeignKey(image => image.BMWId); // Use BMWId as the foreign ke
            // Additional configuration if needed
        }

    }
}