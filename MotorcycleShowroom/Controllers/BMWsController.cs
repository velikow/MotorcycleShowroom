﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MotorcycleShowroom.Data;
using MotorcycleShowroom.Models;
using System.Text.Json;
using System.IO;




namespace MotorcycleShowroom.Controllers
{
  
    public class BMWsController : Controller

    {
        private readonly ILogger<BMWsController> _logger;
        private readonly ApplicationDbContext _context;

        public BMWsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BMWs
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            // Calculate the number of items to skip
            var skipAmount = (page - 1) * pageSize;

            // Retrieve a subset of BMWs with pagination
            var bmwsWithImages = await _context.BMW
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();

            // Get total count of BMWs
            var totalCount = await _context.BMW.CountAsync();

            // Calculate total number of pages
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Pass the subset of BMWs and pagination data to the view
            var Model = new BMWPaginationModel
            {
                BMWs = bmwsWithImages,
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            return View(Model);
        }



        // GET: BMWs/ShowSearchForm
        public IActionResult ShowSearchForm()
        {
            return View();
        }


        // Post: BMWs/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(string searchPhrase)
        {
            // Filter BMWs based on the search phrase
            var filteredBMWs = await _context.BMW
                                        .Where(j => j.Motorcycles.Contains(searchPhrase))
                                        .ToListAsync();

            // Populate the pagination model
            var paginationModel = new BMWPaginationModel
            {
                BMWs = filteredBMWs,
                // Set other properties of the pagination model if needed
            };

            // Pass the pagination model to the Index view
            return View("Index", paginationModel);
        }
        // GET: BMWs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bmw = await _context.BMW
                .Include(j => j.Images)
                                    
                                    .FirstOrDefaultAsync(m => m.Id == id);
            
           // _logger.LogInformation("Received model: {Model}", JsonSerializer.Serialize(bmw));

            if (bmw == null)
            {
                return NotFound();
            }

            return View(bmw);
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
                if (Images == null || Images.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, "Please upload at least one photo.");
                    return View(bMW);
                }
                // Get the current directory of the application
                string currentDirectory = Directory.GetCurrentDirectory();

                // Specify the relative path to the images directory
                string imagesDirectory = "C:/Users/moni_/source/repos/MotorcycleShowroom/MotorcycleShowroom/wwwroot/img/";

                // Create the images directory if it doesn't exist
                if (!Directory.Exists(imagesDirectory))
                {
                    Directory.CreateDirectory(imagesDirectory);
                }
                // Add the BMW object to the context and save changes
                 var bmwinstance = new BMW 
                {
                   Motorcycles= bMW.Motorcycles,
                    Info= bMW.Info
                    
               };
                
                var bmw = await _context.BMW.AddAsync(bmwinstance);
                await _context.SaveChangesAsync();




                foreach (var file in Images)
                {
{
                        // Generate a unique file name for the image
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                        // Combine the images directory with the unique file name
                        string filePath = Path.Combine(imagesDirectory, uniqueFileName);
                        //_logger.LogInformation("Received filePath", filePath);

                        // Save the file to the specified path
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        
                        // Create a new Image object and set its FileName property to the relative image path
                        Image newImage = new Image { FileName = filePath, BMWId = bmwinstance.Id };

                        //_logger.LogInformation("Received Image model: {Model}", JsonSerializer.Serialize(newImage));

                        await _context.Image.AddAsync(newImage);


                       
                        
                    }
                }

                await _context.SaveChangesAsync();


                // Redirect to the Index action method after successfully creating the BMW object
                return RedirectToAction(nameof(Index));
            }

            // If the ModelState is not valid, return the view with the provided BMW object
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
