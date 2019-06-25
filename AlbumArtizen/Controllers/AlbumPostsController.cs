using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlbumArtizen.Data;
using AlbumArtizen.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections;
using AlbumArtizen.Models.AlbumPostViewModel;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;

namespace AlbumArtizen.Controllers
{
    public class AlbumPostsController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly UserManager<ApplicationUser> _userManager;
        public AlbumPostsController(ApplicationDbContext ctx,
                          UserManager<ApplicationUser> userManager,
                          IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _context = ctx;
            _hostingEnvironment = hostingEnvironment;
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        private readonly ApplicationDbContext _context;


        [Authorize]

        // GET: AlbumPosts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AlbumPost.Include(a => a.User);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize]

        // GET: AlbumPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albumPost = await _context.AlbumPost
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AlbumPostId == id);
            if (albumPost == null)
            {
                return NotFound();
            }

            return View(albumPost);
        }

        [Authorize]

        // GET: AlbumPosts/Create
        public IActionResult Create()
        {
            UploadImageViewModel viewalbumpost = new UploadImageViewModel();
            viewalbumpost.AlbumPost = new AlbumPost();
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            return View(viewalbumpost);
        }

        // POST: AlbumPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( UploadImageViewModel viewalbumpost )
        {
            // add current dateTime
            viewalbumpost.AlbumPost.DatePosted = DateTime.Now;
            ModelState.Remove("AlbumPost.UserId");

            // adding current userId
            var user = await GetCurrentUserAsync();
            viewalbumpost.AlbumPost.UserId = user.Id;

            if (ModelState.IsValid)
            {
                    if (viewalbumpost.ImageFile != null)
                    {
                        // don't rely on or trust the FileName property without validation
                        //**Warning**: The following code uses `GetTempFileName`, which throws
                        // an `IOException` if more than 65535 files are created without 
                        // deleting previous temporary files. A real app should either delete
                        // temporary files or use `GetTempPath` and `GetRandomFileName` 
                        // to create temporary file names.
                        var fileName = Path.GetFileName(viewalbumpost.ImageFile.FileName);
                        Path.GetTempFileName();
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await viewalbumpost.ImageFile.CopyToAsync(stream);
                            // validate file, then move to CDN or public folder
                        }

                        viewalbumpost.AlbumPost.ImagePath = viewalbumpost.ImageFile.FileName;
                    }
                    _context.Add(viewalbumpost.AlbumPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", viewalbumpost.AlbumPost.UserId);
            return View(viewalbumpost.AlbumPost);
        }

        [Authorize]

        // GET: AlbumPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albumPost = await _context.AlbumPost.FindAsync(id);
            if (albumPost == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", albumPost.UserId);
            return View(albumPost);
        }

        // POST: AlbumPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AlbumPostId,UserId,DatePosted,Title,ImagePath")] AlbumPost albumPost)
        {
            if (id != albumPost.AlbumPostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(albumPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumPostExists(albumPost.AlbumPostId))
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
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", albumPost.UserId);
            return View(albumPost);
        }

        [Authorize]

        // GET: AlbumPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albumPost = await _context.AlbumPost
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AlbumPostId == id);
            if (albumPost == null)
            {
                return NotFound();
            }

            return View(albumPost);
        }

        // POST: AlbumPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var albumPost = await _context.AlbumPost.FindAsync(id);
            _context.AlbumPost.Remove(albumPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumPostExists(int id)
        {
            return _context.AlbumPost.Any(e => e.AlbumPostId == id);
        }
    }
}
