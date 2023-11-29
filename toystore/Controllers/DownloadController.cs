using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using beadando.Data;
using beadando.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;

namespace toystore.Controllers
{
    [Authorize]
    public class DownloadController : Controller
    {
        private readonly StoreContext _context;

        public DownloadController(StoreContext context)
        {
            _context = context;
        }

        // GET: Download
        public async Task<IActionResult> Index()
        {
            return _context.Downloadables != null
                ? View(await _context.Downloadables.ToListAsync())
                : Problem("Entity set 'StoreContext.Downloadables'  is null.");
        }
        
        [HttpGet]
        public IActionResult Download(int? id) {
            if (id == null || _context.Downloadables == null)
            {
                return NotFound();
            }

            var files = _context.Downloadables
                .FirstOrDefault(m => m.ID == id);
            if (files == null)
            {
                return NotFound();
            }

            return File(System.IO.File.ReadAllBytes(files.Path), 
                "application/force-download", files.Title + "." + files.Path.Split(".").Last());
        }


        // GET: Download/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Downloadables == null)
            {
                return NotFound();
            }

            var downloadable = await _context.Downloadables
                .FirstOrDefaultAsync(m => m.ID == id);
            if (downloadable == null)
            {
                return NotFound();
            }

            return View(downloadable);
        }

        // GET: Download/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Download/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(String title)
        {
            try
            {
                var form = await Request.ReadFormAsync(new FormOptions()
                {
                    BufferBody = false,
                    MultipartBodyLengthLimit = long.MaxValue
                });

                var file = form.Files[0];

                var filePath = Path.Combine("wwwroot", "files", title + "." + file.FileName.Split(".").Last());

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var newFile = new Downloadable
                {
                    Title = title,
                    Path = filePath
                };
                _context.Add(newFile);
                await _context.SaveChangesAsync();

                TempData["message"] = "Fájl sikeresen feltöltve!";
            }
            catch (Exception ex)
            {
                TempData["message"] = "Hiba történt a fájl feltöltése közben: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Download/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Downloadables == null)
            {
                return NotFound();
            }

            var downloadable = await _context.Downloadables
                .FirstOrDefaultAsync(m => m.ID == id);
            if (downloadable == null)
            {
                return NotFound();
            }

            return View(downloadable);
        }

        // POST: Download/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Downloadables == null)
            {
                return Problem("Entity set 'StoreContext.Downloadables'  is null.");
            }

            var downloadable = await _context.Downloadables.FindAsync(id);
            if (downloadable != null)
            {
                if (System.IO.File.Exists(downloadable.Path))
                {
                    System.IO.File.Delete(downloadable.Path);
                }
                _context.Downloadables.Remove(downloadable);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DownloadableExists(int id)
        {
            return (_context.Downloadables?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}