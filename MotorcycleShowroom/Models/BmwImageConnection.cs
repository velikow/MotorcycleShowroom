using System.ComponentModel.DataAnnotations;

namespace MotorcycleShowroom.Models
{
    public class BMWImageConnection
    {
        public int Id { get; set; }
        [Required]

        public int BMWId { get; set; }
        public BMW BMW { get; set; }

        [Required]
        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}
