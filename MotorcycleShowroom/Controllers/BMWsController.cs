using System;
using System.Collections.Generic;
using System.Linq;
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
            return _context.BMW != null ?
                        View(await _context.BMW.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.BMW'  is null.");
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
        public async Task<IActionResult> Create([Bind("Id,Motorcycles,Info")] BMW bMW)
        {
            if (ModelState.IsValid)
            {
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
    }
}
