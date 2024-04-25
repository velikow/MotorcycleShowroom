using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

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


        // Navigation property for user roles
        public UserRole Role { get; set; }
    }
}
