using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace MotorcycleShowroom.Controllers
{
    public class ImageController : Controller
    {
        public IActionResult GetImage(string filePath)
        {
           
            if (System.IO.File.Exists(filePath))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "image/png"); // Adjust MIME type based on actual file type, or dynamically determine it
            }

            return NotFound();
        }

    }

}
   
