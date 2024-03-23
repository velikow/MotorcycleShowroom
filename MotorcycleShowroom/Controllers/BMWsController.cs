using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MotorcycleShowroom.Data;
using MotorcycleShowroom.Models;

namespace MotorcycleShowroom.Controllers
{
    public class BMWsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BMWsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: BMWs
        public async Task<IActionResult> Index()
        {
            // Your raw SQL query
            // string sqlQuery = @"
            // SELECT DISTINCT b.*
            // FROM BMW b
            // LEFT JOIN Image i ON b.Id = i.BMWId
            // "; 
            
            var bmwsWithImages = await _context.BMW
    .Include(bmw => bmw.Images)
    .ToListAsync();


            // Execute the raw SQL query
            //List<BMW> bmws = await _context.BMW.FromSqlRaw(sqlQuery).Include(bmw => bmw.Images).ToListAsync();

            return View(bmwsWithImages);
        }
        // GET: BMWs/ShowSearchForm
        public IActionResult ShowSearchForm()
        {
            return View();
        }

        // Post: BMWs/ShowSearchResults
#pragma warning disable CS8604 // Possible null reference argument.
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase) => View("Index", await _context.BMW.Where(j => j.Motorcycles.Contains(SearchPhrase)).ToListAsync());
#pragma warning restore CS8604 // Possible null reference argument.

        // GET: BMWs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BMW == null)
            {
                return NotFound();
            }

            var bMW = await _context.BMW
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bMW == null)
            {
                return NotFound();
            }

            return View(bMW);
        }

        // GET: BMWs/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BMWs/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Motorcycles,Info,Images")] BMW bMW, List<IFormFile> Images)
        {
            if (ModelState.IsValid)
            {
                
                
                
                string DBPath = Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(Environment.CurrentDirectory)).ToString()).ToString();
                foreach (var file in Images)
                   
                {
                    var filepath = Path.Combine(DBPath,  file.FileName);
                    Console.WriteLine("FilePath=",filepath);
                    Image newImage = new Image();
                    newImage.FileName = filepath;
                    _context.Add(newImage);
                    bMW.Images.Add(newImage);
                   
                    if (file.Length >0 )
                    {
                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        
                    }

                }
                _context.Add(bMW);


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bMW);
        }

        // GET: BMWs/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BMW == null)
            {
                return NotFound();
            }

            var bMW = await _context.BMW.FindAsync(id);
            if (bMW == null)
            {
                return NotFound();
            }
            return View(bMW);
        }

        // POST: BMWs/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Motorcycles,Info")] BMW bMW)
        {
            if (id != bMW.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bMW);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BMWExists(bMW.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bMW);
        }

        // GET: BMWs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BMW == null)
            {
                return NotFound();
            }

            var bMW = await _context.BMW
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bMW == null)
            {
                return NotFound();
            }

            return View(bMW);
        }

        // POST: BMWs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BMW == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BMW'  is null.");
            }
            var bMW = await _context.BMW.FindAsync(id);
            if (bMW != null)
            {
                _context.BMW.Remove(bMW);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BMWExists(int id)
        {
          return (_context.BMW?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public class LikeController : Controller
        {
            private readonly ApplicationDbContext _context;

            public LikeController(ApplicationDbContext context)
            {
                _context = context;
            }

            
            [HttpPost]
            public IActionResult Like(int postId)
            {
            
                var existingLike = _context.Likes.FirstOrDefault(l => l.PostId == postId);

                if (existingLike != null)
                {
                    
                    return Json(new { success = false, message = "You have already liked this post." });
                }

                
                var newLike = new Like
                {
                    PostId = postId,
                    
                };

                
                _context.Likes.Add(newLike);
                _context.SaveChanges();

                
                return Json(new { success = true, message = "Post liked successfully." });
            }

            
            [HttpPost]
            public IActionResult Unlike(int postId)
            {
                
                var existingLike = _context.Likes.FirstOrDefault(l => l.PostId == postId /* && l.UserId == currentUserId if you have user authentication */);

                if (existingLike == null)
                {
                    
                    return Json(new { success = false, message = "Like not found." });
                }

                
                _context.Likes.Remove(existingLike);
                _context.SaveChanges();

                
                return Json(new { success = true, message = "Like removed successfully." });

            }
        }

    }
}
