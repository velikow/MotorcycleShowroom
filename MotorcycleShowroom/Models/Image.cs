using System.ComponentModel.DataAnnotations;

namespace MotorcycleShowroom.Models
{
    public class Image
    {
        public int Id { get; set; }

            [Required]
            public string FileName { get; set; }

            public int BMWId { get; set; }
            public BMW BMW { get; set; }

        public ICollection<Image> Images { get; set; }
    }
    }

