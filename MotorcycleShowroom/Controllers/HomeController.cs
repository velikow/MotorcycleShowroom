using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MotorcycleShowroom.Data;
using MotorcycleShowroom.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MotorcycleShowroom.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            // Fetch all BMW objects with their images
            var bmwsWithImages = await _context.BMW
                    .Include(b => b.Images)
                    .ToListAsync();

            return View(bmwsWithImages);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
