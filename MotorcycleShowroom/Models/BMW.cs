using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
namespace MotorcycleShowroom.Models
{
    public class BMW
    {
        
        public int Id { get; set; }
        public string Motorcycles { get; set; }
        public string Info { get; set; }
        public string? UserId { get; set; } // Add UserId property

        // Navigation property for the related images
        public ICollection<Image> Images { get; set; }

        public BMW()
        {

        }
    }
}
