using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlbumArtizen.Data;
using AlbumArtizen.Models;

namespace AlbumArtizen.Controllers
{
    public class SavedsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SavedsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Saveds
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Saved.Include(a => a.User);

            return View(await applicationDbContext.ToListAsync());

        }

        // GET: Saveds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saved = await _context.Saved
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.SavedId == id);
            if (saved == null)
            {
                return NotFound();
            }

            return View(saved);
        }

        // GET: Saveds/Create
        public IActionResult Create()
        {



            var applicationDbContext = _context.AlbumPost.Include(a => a.User);

            return View(applicationDbContext.ToListAsync());

        }

        // POST: Saveds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SavedId,DateCreated,UserId")] Saved saved)
        {
            if (ModelState.IsValid)
            {
                _context.Add(saved);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", saved.UserId);
            return View(saved);
        }

        // GET: Saveds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saved = await _context.Saved.FindAsync(id);
            if (saved == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", saved.UserId);
            return View(saved);
        }

        // POST: Saveds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SavedId,DateCreated,UserId")] Saved saved)
        {
            if (id != saved.SavedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(saved);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SavedExists(saved.SavedId))
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
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", saved.UserId);
            return View(saved);
        }

        // GET: Saveds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saved = await _context.Saved
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.SavedId == id);
            if (saved == null)
            {
                return NotFound();
            }

            return View(saved);
        }

        // POST: Saveds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var saved = await _context.Saved.FindAsync(id);
            _context.Saved.Remove(saved);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SavedExists(int id)
        {
            return _context.Saved.Any(e => e.SavedId == id);
        }
    }
}
