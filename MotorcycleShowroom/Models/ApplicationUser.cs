using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MotorcycleShowroom.Models
{
    public enum UserRole
    {
        SuperUser,
        NormalUser
    }
    public class ApplicationUser : IdentityUser
    {
        // Navigation property for likes
        public ICollection<Like> Likes { get; set; } = new List<Like>();

        public int Id { get; set; }
        // Navigation property for user roles
        public UserRole Role { get; set; }
    }
}
