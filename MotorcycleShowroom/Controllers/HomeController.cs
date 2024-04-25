using Microsoft.AspNetCore.Identity;
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
       
        public async Task<IActionResult> IndexAsync(string searchPhrase)
        {
            // Query all BMWs with their images included
            var bmwsWithImagesQuery = _context.BMW.Include(b => b.Images);

            // If searchPhrase is null or empty, return all motorcycles
            if (string.IsNullOrEmpty(searchPhrase))
            {
                var allBMWsWithImages = await bmwsWithImagesQuery.ToListAsync();
                return View(allBMWsWithImages);
            }
            else
            {
                // Filter motorcycles by searchPhrase
                var filteredBMWsWithImages = await bmwsWithImagesQuery
                    .Where(j => j.Motorcycles.Contains(searchPhrase))
                    .ToListAsync();

                return View(filteredBMWsWithImages);
            }
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
